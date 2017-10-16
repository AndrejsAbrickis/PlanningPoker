using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace signalR_demo
{

    public class InMemoryUserTracker<THub> : IUserTracker<THub>
    {
        private readonly ConcurrentDictionary<HubConnectionContext, UserDetails> _usersOnline
            = new ConcurrentDictionary<HubConnectionContext, UserDetails>();

        public event Action<UserDetails[]> UsersJoined;
        public event Action<UserDetails[]> UsersLeft;

        public Task<IEnumerable<UserDetails>> UsersOnline() => Task.FromResult(_usersOnline.Values.AsEnumerable());

        public Task<IEnumerable<UserDetails>> UsersOnline(string groupId)
        {
            return Task.FromResult(_usersOnline.Values.Where(u => u.GroupId == groupId).AsEnumerable());
        }

        public Task<UserDetails> GetUser(HubConnectionContext connection)
        {
            var user = _usersOnline.Values.FirstOrDefault( u => u.ConnectionId == connection.ConnectionId);

            return Task.FromResult(user);
        }

        public Task AddUser(HubConnectionContext connection, UserDetails userDetails)
        {
            _usersOnline.TryAdd(connection, userDetails);
            UsersJoined(new[] { userDetails });

            return Task.CompletedTask;
        }

        public Task UpdateUser(HubConnectionContext connection, UserDetails userDetails)
        {
            _usersOnline.AddOrUpdate(connection, userDetails, (oldKey, oldValue) => userDetails);

            return Task.CompletedTask;
        }

        public Task RemoveUser(HubConnectionContext connection)
        {
            if (_usersOnline.TryRemove(connection, out var userDetails))
            {
                UsersLeft(new[] { userDetails });
            }

            return Task.CompletedTask;
        }
    }
}