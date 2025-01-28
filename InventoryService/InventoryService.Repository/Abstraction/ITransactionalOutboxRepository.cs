using InventoryService.Repository.Model;

namespace InventoryService.Repository.Abstraction
{
    public interface ITransactionalOutboxRepository
    {
        Task AddMessageAsync(TransactionalOutbox message);
        Task<IEnumerable<TransactionalOutbox>> GetPendingMessagesAsync();
        Task RemoveMessageAsync(int id);
    }
}
