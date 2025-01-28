using InventoryService.Repository.Abstraction;
using InventoryService.Repository.Model;
using Microsoft.EntityFrameworkCore;

namespace InventoryService.Repository
{
    public class TransactionalOutboxRepository : ITransactionalOutboxRepository
    {
        private readonly InventoryDbContext _context;

        public TransactionalOutboxRepository(InventoryDbContext context)
        {
            _context = context;
        }

        public async Task AddMessageAsync(TransactionalOutbox message)
        {
            await _context.Set<TransactionalOutbox>().AddAsync(message);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<TransactionalOutbox>> GetPendingMessagesAsync()
        {
            return await _context.Set<TransactionalOutbox>().ToListAsync();
        }

        public async Task RemoveMessageAsync(int id)
        {
            var message = await _context.Set<TransactionalOutbox>().FindAsync(id);
            if (message != null)
            {
                _context.Set<TransactionalOutbox>().Remove(message);
                await _context.SaveChangesAsync();
            }
        }
    }
}
