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
        
        public Task AddUser(HubConnectionContext connection, UserDetails userDetails)
        {
            _usersOnline.TryAdd(connection, userDetails);
            UsersJoined(new[] { userDetails });

            return Task.CompletedTask;
        }

        public Task UpdateUser(HubConnectionContext connection, UserDetails userDetails)
        {
            _usersOnline.AddOrUpdate(connection, userDetails, (odlKey, oldValue) => userDetails);

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