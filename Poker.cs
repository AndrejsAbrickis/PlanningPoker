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
            await Clients.Client(Context.ConnectionId).InvokeAsync(HubEvents.Connected, await GetUsersOnline());

            await base.OnConnectedAsync();
        }

        public override Task OnUsersJoined(UserDetails[] users)
        {
            return Clients.Client(Context.ConnectionId).InvokeAsync(HubEvents.UsersJoined, users);
        }

        public override async Task OnUsersLeft(UserDetails[] users)
        {
            await Clients.Client(Context.ConnectionId).InvokeAsync(HubEvents.Disconnected, await GetUsersOnline());

            await base.OnUsersLeft(users);
        }

        public async Task Send(string message)
        {
            var pokerMessage = new PokerMessage(Context.Connection.ConnectionId, message);
            await Clients.All.InvokeAsync(HubEvents.Send, pokerMessage);
        }

        public async Task JoinUser(string userName)
        {
            var userDetails = new UserDetails(Context.Connection.ConnectionId, userName);
            await _userTracker.UpdateUser(Context.Connection, userDetails);

            await Clients.All.InvokeAsync(HubEvents.JoinUser, userDetails);
        }

        public async Task NewGame()
        {
            await Clients.All.InvokeAsync(HubEvents.NewGame);
        }
        
        public async Task ShowCards()
        {
            await Clients.All.InvokeAsync(HubEvents.ShowCards);
        }
    }

    public class HubEvents
    {
        internal const string Send = "Send";
        internal const string UsersJoined = "UsersJoined";
        internal const string Disconnected = "Disconnected";
        internal const string Connected = "Connected";
        internal const string JoinUser = "JoinUser";
        internal const string NewGame = "NewGame";
        internal const string ShowCards = "ShowCards";
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