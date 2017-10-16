using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using signalR_demo;

namespace signalR_demo.Pages
{
    public class DebugModel : PageModel
    {
        protected IUserTracker<HubWithPresence> _userTracker;
        public IEnumerable<UserDetails> Users;

        public DebugModel(IUserTracker<HubWithPresence> userTracker)
        {
            _userTracker = userTracker;
        }

        public async System.Threading.Tasks.Task<IActionResult> OnGetAsync()
        {
            Users = await _userTracker.UsersOnline();

            return Page();
        }
    }
}