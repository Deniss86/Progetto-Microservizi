namespace InventoryService.Shared.DTOs
{
    public class OutboxMessageDto
    {
        public string Table { get; set; } = string.Empty; // Nome della tabella associata al messaggio
        public string Operation { get; set; } = string.Empty; // Tipo di operazione: Insert, Update, Delete
        public string Message { get; set; } = string.Empty; // Messaggio serializzato
    }
}
