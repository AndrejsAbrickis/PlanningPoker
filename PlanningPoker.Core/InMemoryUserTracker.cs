using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using PlanningPoker.Data.DTOs;

namespace PlanningPoker.Core
{

    public class InMemoryUserTracker<THub> : IUserTracker<THub>
    {
        private readonly ConcurrentDictionary<string, UserDetailsDto> _usersOnline
            = new ConcurrentDictionary<string, UserDetailsDto>();

        public event Action<UserDetailsDto[]> UsersJoined;
        public event Action<UserDetailsDto[]> UsersLeft;

        public Task<IEnumerable<UserDetailsDto>> UsersOnline() => Task.FromResult(_usersOnline.Values.AsEnumerable());

        public Task<IEnumerable<UserDetailsDto>> UsersOnline(string groupId)
        {
            return Task.FromResult(_usersOnline.Values.Where(u => u.GroupId == groupId).AsEnumerable());
        }

        public Task<UserDetailsDto> GetUser(string connectionId)
        {
            var user = _usersOnline.Values.FirstOrDefault( u => u.ConnectionId == connectionId);

            return Task.FromResult(user);
        }

        public Task AddUser(string connectionId, UserDetailsDto userDetails)
        {
            _usersOnline.TryAdd(connectionId, userDetails);
            UsersJoined(new[] { userDetails });

            return Task.CompletedTask;
        }

        public Task UpdateUser(string connectionId, UserDetailsDto userDetails)
        {
            _usersOnline.AddOrUpdate(connectionId, userDetails, (oldKey, oldValue) => userDetails);

            return Task.CompletedTask;
        }

        public Task RemoveUser(string connectionId)
        {
            if (_usersOnline.TryRemove(connectionId, out var userDetails))
            {
                UsersLeft(new[] { userDetails });
            }

            return Task.CompletedTask;
        }
    }
}