namespace InventoryService.Repository.Model
{
    public class TransactionalOutbox
    {
        public int Id { get; set; }
        public string Tabella { get; set; } // Nome della tabella modificata
        public string Messaggio { get; set; }
    }
}
