namespace PlanningPoker.Web
{
    public class UserDetails
    {
        public UserDetails(string connectionId, string name, string groupId)
        {
            ConnectionId = connectionId;
            Name = name;
            GroupId = groupId;
        }

        public string ConnectionId { get; set; }
        public string Name { get; set; }
        public string GroupId { get; set; }
    }
}