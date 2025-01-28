namespace OrderService.Repository.Abstraction
{
    public interface ITransactionalOutboxRepository
    {
        Task AddToOutboxAsync(string tableName, string payload, string eventType);
        Task RemoveProcessedMessagesAsync();
    }
}
