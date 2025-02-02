/*using System.Text.Json; // Importa il supporto per la serializzazione JSON
using InventoryService.Repository.Abstraction; // Importa il repository della tabella Outbox
using InventoryService.Shared.DTOs; // Importa i Data Transfer Objects (DTO)

namespace InventoryService.Kafka
{
    // Implementazione del producer Kafka
    public class KafkaProducer : IKafkaProducer
    {
        private readonly string _bootstrapServers; // Indirizzo del server Kafka
        private readonly ITransactionalOutboxRepository _outboxRepository; // Repository per i messaggi da pubblicare

        // Costruttore con Dependency Injection
        public KafkaProducer(string bootstrapServers, ITransactionalOutboxRepository outboxRepository)
        {
            _bootstrapServers = bootstrapServers;
            _outboxRepository = outboxRepository;
        }

        // Aggiunge un messaggio all'outbox prima di inviarlo su Kafka
        public async Task AddToOutboxAsync<T>(T message, string topic)
        {
            var serializedMessage = JsonSerializer.Serialize(message); // Serializza il messaggio in JSON

            await _outboxRepository.AddToOutboxAsync(new TransactionalOutbox
            {
                Topic = topic, // Specifica il topic Kafka
                Message = serializedMessage // Salva il messaggio nella tabella outbox
            });
        }

        // Pubblica i messaggi dalla tabella di outbox su Kafka
        public async Task PublishMessagesFromOutboxAsync()
        {
            var messages = await _outboxRepository.GetUnpublishedMessagesAsync(); // Recupera i messaggi non ancora pubblicati

            foreach (var message in messages)
            {
                // Crea un producer Kafka
                using var producer = new Confluent.Kafka.ProducerBuilder<Null, string>(
                    new Confluent.Kafka.ProducerConfig { BootstrapServers = _bootstrapServers }).Build();

                // Pubblica il messaggio sul topic specificato
                await producer.ProduceAsync(message.Topic, new Confluent.Kafka.Message<Null, string>
                {
                    Value = message.Message
                });

                // Rimuove il messaggio dall'outbox dopo la pubblicazione
                await _outboxRepository.RemoveFromOutboxAsync(message.Id);
            }
        }
    }
}
