/*
using OrderService.Repository.Abstraction; // Importa l'interfaccia del repository dell'outbox transazionale
using OrderService.Repository.Model; // Importa il modello della tabella TransactionalOutbox
using Microsoft.EntityFrameworkCore; // Importa Entity Framework Core per la gestione del database
/*
Un nuovo evento viene generato (es. creazione ordine).
Il messaggio viene prima salvato nella tabella TransactionalOutbox invece di inviarlo immediatamente a Kafka.
Un processo separato (Kafka Producer) legge i messaggi dall'outbox e li invia a Kafka.
Dopo che Kafka ha ricevuto il messaggio, il record viene eliminato dall'outbox.
*/
/*
namespace OrderService.Repository
{
    // Implementa l'interfaccia ITransactionalOutboxRepository per la gestione dell'outbox
    public class TransactionalOutboxRepository : ITransactionalOutboxRepository
    {
        private readonly OrderDbContext _context; // Istanza del contesto del database

        // Costruttore che riceve OrderDbContext tramite Dependency Injection
        public TransactionalOutboxRepository(OrderDbContext context)
        {
            _context = context;
        }

        // Metodo per aggiungere un nuovo messaggio all'outbox
        public async Task AddToOutboxAsync(string tableName, string payload, string eventType)
        {
            // Crea un nuovo record per la tabella TransactionalOutbox
            var outboxEntry = new TransactionalOutbox
            {
                TableName = tableName, // Nome della tabella correlata al messaggio (es. Orders)
                Payload = payload, // Contenuto del messaggio serializzato in JSON
                EventType = eventType // Tipo di evento (Insert, Update, Delete)
            };

            // Aggiunge il nuovo messaggio all'outbox nel database
            _context.TransactionalOutbox.Add(outboxEntry);

            // Salva le modifiche nel database per rendere persistente il messaggio
            await _context.SaveChangesAsync();
        }

        // Metodo per rimuovere tutti i messaggi già elaborati dall'outbox
        public async Task RemoveProcessedMessagesAsync()
        {
            // Recupera tutti i messaggi presenti nella tabella TransactionalOutbox
            var processedMessages = await _context.TransactionalOutbox.ToListAsync();

            // Rimuove tutti i messaggi recuperati, dopo che sono stati inviati a Kafka
            _context.TransactionalOutbox.RemoveRange(processedMessages);

            // Salva le modifiche nel database
            await _context.SaveChangesAsync();
        }
    }
}
*/