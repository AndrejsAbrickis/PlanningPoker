using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace signalR_demo
{
    public class Poker : Hub
    {
        public Task Send(string message)
        {
            return Clients.All.InvokeAsync(HubEvents.Send, $"Message from server: {message}");
        }

        // public override async Task OnConnectedAsync()
        // {
        //     await Clients.Client(Context.ConnectionId).InvokeAsync("SetUsersOnline", await GetUsersOnline());

        //     await base.OnConnectedAsync();
        // }

        public Task OnUsersJoined(string users)
        {
            return Clients.All.InvokeAsync(HubEvents.UsersJoined, users);
        }

        public Task OnUsersLeft(string users)
        {
            return Clients.All.InvokeAsync(HubEvents.UsersLeft, users);
        }
    }

    public class HubEvents
    {
        public const string Send = "Send";
        public const string UsersJoined = "UsersJoined";
        public const string UsersLeft = "UsersLeft";
    }
}