using System.Text.Json;
using InventoryService.Repository.Abstraction;
using InventoryService.Shared.DTOs;

namespace InventoryService.Kafka
{
    public class KafkaProducer : IKafkaProducer
    {
        private readonly string _bootstrapServers;
        private readonly ITransactionalOutboxRepository _outboxRepository;

        public KafkaProducer(string bootstrapServers, ITransactionalOutboxRepository outboxRepository)
        {
            _bootstrapServers = bootstrapServers;
            _outboxRepository = outboxRepository;
        }

        public async Task AddToOutboxAsync<T>(T message, string topic)
        {
            var serializedMessage = JsonSerializer.Serialize(message);

            await _outboxRepository.AddToOutboxAsync(new TransactionalOutbox
            {
                Topic = topic,
                Message = serializedMessage
            });
        }

        public async Task PublishMessagesFromOutboxAsync()
        {
            var messages = await _outboxRepository.GetUnpublishedMessagesAsync();

            foreach (var message in messages)
            {
                using var producer = new Confluent.Kafka.ProducerBuilder<Null, string>(
                    new Confluent.Kafka.ProducerConfig { BootstrapServers = _bootstrapServers }).Build();

                await producer.ProduceAsync(message.Topic, new Confluent.Kafka.Message<Null, string>
                {
                    Value = message.Message
                });

                await _outboxRepository.RemoveFromOutboxAsync(message.Id);
            }
        }
    }
}