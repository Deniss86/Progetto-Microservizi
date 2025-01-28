namespace OrderService.Repository.Model
{
    public class TransactionalOutbox
    {
        public int Id { get; set; }
        public string Tabella { get; set; } // Nome della tabella interessata
        public string Messaggio { get; set; } // Serializzazione del DTO
    }
}
