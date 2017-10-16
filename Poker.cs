using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace signalR_vuejs_demo
{
    public class Poker : HubWithPresence
    {
        public Poker(IUserTracker<Poker> userTracker) : base(userTracker)
        {
        }

        public override async Task OnConnectedAsync()
        {
            var user = await _userTracker.GetUser(Context.Connection);
            var usersOnline = await GetUsersOnline();
            var groupUsersOnline = usersOnline.Where(u => u.GroupId == user.GroupId);

            await Clients.Client(Context.ConnectionId).InvokeAsync(HubEvents.Connected, groupUsersOnline);

            await base.OnConnectedAsync();
        }

        public override async Task OnUsersLeft(UserDetails[] users)
        {
            var user = await _userTracker.GetUser(Context.Connection);
            var usersOnline = await GetUsersOnline();
            var groupUsersOnline = usersOnline.Where(u => u.GroupId == user.GroupId);

            await Clients.Group(user.GroupId).InvokeAsync(HubEvents.Disconnected, groupUsersOnline);

            await base.OnUsersLeft(users);
        }

        public async Task Send(string message)
        {
            var pokerMessage = new PokerMessage(Context.Connection.ConnectionId, message);

            var user = await _userTracker.GetUser(Context.Connection);
            await Clients.Group(user.GroupId).InvokeAsync(HubEvents.Send, pokerMessage);
        }

        public async Task JoinUser(string userName)
        {
            var user = await _userTracker.GetUser(Context.Connection);
            user.Name = userName;

            await _userTracker.UpdateUser(Context.Connection, user);
            await Clients.Group(user.GroupId).InvokeAsync(HubEvents.JoinUser, user);
        }

        public async Task NewGame()
        {
            var user = await _userTracker.GetUser(Context.Connection);
            await Clients.Group(user.GroupId).InvokeAsync(HubEvents.NewGame);
        }

        public async Task ShowCards()
        {
            var user = await _userTracker.GetUser(Context.Connection);
            await Clients.Group(user.GroupId).InvokeAsync(HubEvents.ShowCards);
        }

        public async Task JoinGroup(GroupMessage groupMessage)
        {
            var user = await _userTracker.GetUser(Context.Connection);
            user.Name = groupMessage.PlayerName;

            var groupId = groupMessage?.GroupId ?? user.GroupId;
            user.GroupId = groupId;

            var usersOnline = await GetUsersOnline();
            var groupUsersOnline = usersOnline.Where(u => u.GroupId == user.GroupId);

            await _userTracker.UpdateUser(Context.Connection, user);
            await Groups.AddAsync(Context.ConnectionId, groupId);
            await Clients.Group(groupId).InvokeAsync(HubEvents.JoinGroup, groupUsersOnline);
            await Clients.Client(Context.ConnectionId).InvokeAsync(HubEvents.UpdateUser, user);
        }

        public async Task LeaveGroup(string groupName)
        {
            await Clients.Group(groupName).InvokeAsync(HubEvents.LeaveGroup, groupName);
            await Groups.RemoveAsync(Context.ConnectionId, groupName);
        }
    }

    public class HubEvents
    {
        public const string Send = "Send";
        public const string UsersJoined = "UsersJoined";
        public const string Disconnected = "Disconnected";
        public const string Connected = "Connected";
        public const string JoinUser = "JoinUser";
        public const string NewGame = "NewGame";
        public const string ShowCards = "ShowCards";
        public const string JoinGroup = "JoinGroup";
        public const string LeaveGroup = "LeaveGroup";
        public const string UpdateUser = "UpdateUser";
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

    public class GroupMessage
    {
        public string PlayerName { get; set; }
        public string GroupId { get; set; }
    }
}