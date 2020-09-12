using System.Collections.Generic;
using System.Linq;
using PlanningPoker.Data.DTOs;

namespace PlanningPoker.Web.Hubs
{
  public static class ConnectedUsers
  {
    public static IList<GameUser> GameUsers = new List<GameUser>();

    public static IEnumerable<GameUser> GetByGroup(string id) => GameUsers.Where(u => u.GroupId.Equals(id));
    public static GameUser GetByConnectionId(string id) => GameUsers.First(u => u.ConnectionId.Equals(id));
  }
}