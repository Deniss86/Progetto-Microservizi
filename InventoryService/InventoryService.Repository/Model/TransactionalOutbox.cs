namespace InventoryService.Repository.Model
{
    // TransactionalOutbox è una tabella per garantire che i messaggi Kafka vengano pubblicati in modo affidabile.
    public class TransactionalOutbox
    {
        public int Id { get; set; } // Identificativo univoco del messaggio
        public string? Table { get; set; } // Nome della tabella modificata
        public string? Message{ get; set; } // Contenuto del messaggio JSON
    }
}
