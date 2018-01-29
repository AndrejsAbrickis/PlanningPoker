namespace PlanningPoker.Data.DTOs
{
    public class UserDetailsDto
    {
        public UserDetailsDto(string connectionId, string name, string groupId)
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