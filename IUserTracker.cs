using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace signalR_vuejs_demo
{
    public interface IUserTracker<out THub>
    {
        Task<IEnumerable<UserDetails>> UsersOnline();
        Task<IEnumerable<UserDetails>> UsersOnline(string groupId);
        Task<UserDetails> GetUser(HubConnectionContext connection);
        Task AddUser(HubConnectionContext connection, UserDetails userDetails);
        Task UpdateUser(HubConnectionContext connection, UserDetails userDetails);
        Task RemoveUser(HubConnectionContext connection);

        event Action<UserDetails[]> UsersJoined;
        event Action<UserDetails[]> UsersLeft;
    }
}