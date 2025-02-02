namespace InventoryService.Shared.DTOs
{
    // DTO per rappresentare i messaggi nella tabella di outbox
    public class OutboxMessageDto
    {
        public string Table { get; set; } = string.Empty; // Nome della tabella associata al messaggio
        public string Operation { get; set; } = string.Empty; // Tipo di operazione: Insert, Update, Delete
        public string Message { get; set; } = string.Empty; // Messaggio serializzato in formato JSON
    }
}
/*
OutboxMessageDto è un Data Transfer Object (DTO) che rappresenta un messaggio salvato nella tabella TransactionalOutbox.
Scopo:
Questo DTO viene utilizzato per trasferire informazioni tra il livello di business e il repository TransactionalOutbox.
I messaggi vengono salvati temporaneamente nella tabella TransactionalOutbox e poi inviati a Kafka.
*/