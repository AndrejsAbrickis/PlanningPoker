using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using PlanningPoker.Data.DTOs;

namespace PlanningPoker.Core
{
    public interface IUserTracker<out THub>
    {
        Task<IEnumerable<UserDetailsDto>> UsersOnline();
        Task<UserDetailsDto> GetUser(string connectionId);
        Task AddUser(string connectionId, UserDetailsDto userDetails);
        Task UpdateUser(string connectionId, UserDetailsDto userDetails);
        Task RemoveUser(string connectionId);

        event Action<UserDetailsDto[]> UsersJoined;
        event Action<UserDetailsDto[]> UsersLeft;
    }
}