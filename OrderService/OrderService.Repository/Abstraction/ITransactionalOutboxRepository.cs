namespace OrderService.Repository.Abstraction
{
    // Definisce un'interfaccia per la gestione dell'outbox transazionale
    public interface ITransactionalOutboxRepository
    {
        // Metodo per aggiungere un messaggio all'outbox
        Task AddToOutboxAsync(string tableName, string payload, string eventType);

        // Metodo per rimuovere i messaggi elaborati
        Task RemoveProcessedMessagesAsync();
    }
}
