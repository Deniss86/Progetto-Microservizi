using InventoryService.Repository.Abstraction; // Importa l'interfaccia del repository
using InventoryService.Repository.Model; // Importa il modello TransactionalOutbox
using Microsoft.EntityFrameworkCore; // Importa Entity Framework Core per la gestione del database

namespace InventoryService.Repository
{
    // Implementazione del repository per la gestione della tabella TransactionalOutbox
    public class TransactionalOutboxRepository : ITransactionalOutboxRepository
    {
        private readonly InventoryDbContext _context; // Istanza del contesto del database

        // Costruttore che riceve il contesto database tramite Dependency Injection
        public TransactionalOutboxRepository(InventoryDbContext context)
        {
            _context = context;
        }

        // Metodo per aggiungere un messaggio alla tabella di outbox
        public async Task AddMessageAsync(TransactionalOutbox message)
        {
            await _context.Set<TransactionalOutbox>().AddAsync(message); // Aggiunge il messaggio al database
            await _context.SaveChangesAsync(); // Salva le modifiche
        }

        // Metodo per ottenere tutti i messaggi in attesa di essere elaborati
        public async Task<IEnumerable<TransactionalOutbox>> GetPendingMessagesAsync()
        {
            return await _context.Set<TransactionalOutbox>().ToListAsync(); // Recupera tutti i messaggi
        }

        // Metodo per rimuovere un messaggio dall'outbox dopo l'elaborazione
        public async Task RemoveMessageAsync(int id)
        {
            var message = await _context.Set<TransactionalOutbox>().FindAsync(id); // Cerca il messaggio per ID
            if (message != null)
            {
                _context.Set<TransactionalOutbox>().Remove(message); // Rimuove il messaggio
                await _context.SaveChangesAsync(); // Salva le modifiche
            }
        }
    }
}
