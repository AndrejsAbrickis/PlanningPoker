using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace PlanningPoker.Web
{
    public class HubWithPresence : Hub
    {
        protected IUserTracker<HubWithPresence> _userTracker;

        public HubWithPresence(IUserTracker<HubWithPresence> userTracker)
        {
            _userTracker = userTracker;
        }

        public Task<IEnumerable<UserDetails>> GetUsersOnline()
        {
            return _userTracker.UsersOnline();
        }

        public virtual Task OnUsersJoined(UserDetails[] user)
        {
            return Task.CompletedTask;
        }

        public virtual Task OnUsersLeft(UserDetails[] user)
        {
            return Task.CompletedTask;
        }
    }
}