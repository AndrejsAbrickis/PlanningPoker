namespace PlanningPoker.Data.DTOs
{
    public class PokerMessageDto
    {
        public PokerMessageDto(string connectionId, string message)
        {
            ConnectionId = connectionId;
            Message = message;
        }
        public string ConnectionId { get; }
        public string Message { get; }
    }
}