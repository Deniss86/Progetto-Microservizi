using InventoryService.Repository.Model; // Importa il modello TransactionalOutbox

namespace InventoryService.Repository.Abstraction
{
    // Definisce un'interfaccia per gestire i messaggi di outbox transazionale
    public interface ITransactionalOutboxRepository
    {
        // Metodo per aggiungere un nuovo messaggio all'outbox
        Task AddMessageAsync(TransactionalOutbox message);

        // Metodo per ottenere i messaggi non ancora elaborati
        Task<IEnumerable<TransactionalOutbox>> GetPendingMessagesAsync();

        // Metodo per rimuovere un messaggio dall'outbox
        Task RemoveMessageAsync(int id);
    }
}
