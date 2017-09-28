using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace signalR_demo
{
    public class Poker : HubWithPresence
    {
        public Poker(IUserTracker<Poker> userTracker) : base(userTracker)
        {
        }

        public override async Task OnConnectedAsync()
        {
            await Clients.Client(Context.ConnectionId).InvokeAsync(HubEvents.SetUsersOnline, await GetUsersOnline());

            await base.OnConnectedAsync();
        }

        public override Task OnUsersJoined(UserDetails[] users)
        {
            return Clients.Client(Context.ConnectionId).InvokeAsync(HubEvents.UsersJoined, users);
        }

        public override Task OnUsersLeft(UserDetails[] users)
        {
            return Clients.Client(Context.ConnectionId).InvokeAsync(HubEvents.UsersLeft, users);
        }

        public async Task Send(string message)
        {
            //var allUsersOnline = await _userTracker.UsersOnline();
            var pokerMessage = new PokerMessage(Context.Connection.ConnectionId, message);
            await Clients.All.InvokeAsync(HubEvents.Send, pokerMessage);
        }

        public async Task JoinUser(string userName)
        {
            var userDetails = new UserDetails(Context.Connection.ConnectionId, userName);
            await _userTracker.UpdateUser(Context.Connection, userDetails);

            await Clients.All.InvokeAsync(HubEvents.JoinUser, userDetails);
        }
    }

    public class HubEvents
    {
        public const string Send = "Send";
        public const string UsersJoined = "UsersJoined";
        public const string UsersLeft = "UsersLeft";
        public const string SetUsersOnline = "SetUsersOnline";
        public const string JoinUser = "JoinUser";
    }

    public class PokerMessage
    {
        public PokerMessage(string connectionId, string message)
        {
            ConnectionId = connectionId;
            Message = message;
        }
        public string ConnectionId { get; }
        public string Message { get; }
    }
}