namespace OrderService.Shared.Models
{
    // DTO per la gestione dei messaggi dell'outbox degli ordini
    //Questo DTO viene usato per salvare gli eventi nella tabella TransactionalOutbox.
    //È consumato da KafkaProducer.cs per inviare messaggi a Kafka.
    public class OrderOutboxMessageDto
    {
        public int OrderId { get; set; } // ID dell'ordine associato al messaggio
        public string EventType { get; set; } = string.Empty; // Tipo di evento: "Insert", "Update", "Delete"
        public string Payload { get; set; } = string.Empty; // Dati dell'ordine in formato JSON
    }
}
