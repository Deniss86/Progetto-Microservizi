using OrderService.Repository.Abstraction;
using OrderService.Repository.Model;
using Microsoft.EntityFrameworkCore;

namespace OrderService.Repository
{
    public class TransactionalOutboxRepository : ITransactionalOutboxRepository
    {
        private readonly OrderDbContext _context;

        public TransactionalOutboxRepository(OrderDbContext context)
        {
            _context = context;
        }

        public async Task AddToOutboxAsync(string tableName, string payload, string eventType)
        {
            var outboxEntry = new TransactionalOutbox
            {
                TableName = tableName,
                Payload = payload,
                EventType = eventType
            };
            _context.TransactionalOutbox.Add(outboxEntry);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveProcessedMessagesAsync()
        {
            var processedMessages = await _context.TransactionalOutbox.ToListAsync();
            _context.TransactionalOutbox.RemoveRange(processedMessages);
            await _context.SaveChangesAsync();
        }
    }
}
