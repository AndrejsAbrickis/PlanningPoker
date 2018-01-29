using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using PlanningPoker.Data.DTOs;

namespace PlanningPoker.Web
{
    public class HubWithPresence : Hub
    {
        protected IUserTracker<HubWithPresence> _userTracker;

        public HubWithPresence(IUserTracker<HubWithPresence> userTracker)
        {
            _userTracker = userTracker;
        }

        public Task<IEnumerable<UserDetailsDto>> GetUsersOnline()
        {
            return _userTracker.UsersOnline();
        }

        public virtual Task OnUsersJoined(UserDetailsDto[] user)
        {
            return Task.CompletedTask;
        }

        public virtual Task OnUsersLeft(UserDetailsDto[] user)
        {
            return Task.CompletedTask;
        }
    }
}