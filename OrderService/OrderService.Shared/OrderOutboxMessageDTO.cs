namespace OrderService.Shared.Models
{
    public class OrderOutboxMessageDto
    {
        public int OrderId { get; set; }
        public string EventType { get; set; } = string.Empty; // Insert, Update, Delete
        public string Payload { get; set; } = string.Empty;
    }
}
