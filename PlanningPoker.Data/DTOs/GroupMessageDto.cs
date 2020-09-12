namespace PlanningPoker.Data.DTOs
{
    public class GroupMessageDto
    {
        public string PlayerName { get; set; }
        public string GroupId { get; set; }
    }
    public class JoinGameRequestDto
    {
        public string Name { get; set; }
        public string GroupId { get; set; }
    }

    public class GameUser
    {
        public string Name { get; set; }
        public string ConnectionId { get; set; }
        public string GroupId { get; set; }
    }

    public class CardDTO {
        public string ConnectionId { get; set; }
        public string Card { get; set; }
    }
}