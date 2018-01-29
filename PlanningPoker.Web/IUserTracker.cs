using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using PlanningPoker.Data.DTOs;

namespace PlanningPoker.Web
{
    public interface IUserTracker<out THub>
    {
        Task<IEnumerable<UserDetailsDto>> UsersOnline();
        Task<IEnumerable<UserDetailsDto>> UsersOnline(string groupId);
        Task<UserDetailsDto> GetUser(HubConnectionContext connection);
        Task AddUser(HubConnectionContext connection, UserDetailsDto userDetails);
        Task UpdateUser(HubConnectionContext connection, UserDetailsDto userDetails);
        Task RemoveUser(HubConnectionContext connection);

        event Action<UserDetailsDto[]> UsersJoined;
        event Action<UserDetailsDto[]> UsersLeft;
    }
}