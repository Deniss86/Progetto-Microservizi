using System.Text.Json;
using Confluent.Kafka;
using InventoryService.Repository.Abstraction;
using InventoryService.Shared.DTOs;

namespace InventoryService.Kafka
{
    public class KafkaConsumer : IKafkaConsumer
    {
        private readonly string _bootstrapServers;
        private readonly string _groupId;
        private readonly IProductRepository _productRepository;

        public KafkaConsumer(string bootstrapServers, string groupId, IProductRepository productRepository)
        {
            _bootstrapServers = bootstrapServers;
            _groupId = groupId;
            _productRepository = productRepository;
        }

        public void Consume(string topic, CancellationToken cancellationToken)
        {
            var config = new ConsumerConfig
            {
                BootstrapServers = _bootstrapServers,
                GroupId = _groupId,
                AutoOffsetReset = AutoOffsetReset.Earliest
            };

            using var consumer = new ConsumerBuilder<Ignore, string>(config).Build();
            consumer.Subscribe(topic);

            try
            {
                while (!cancellationToken.IsCancellationRequested)
                {
                    var result = consumer.Consume(cancellationToken);

                    if (result != null)
                    {
                        var message = JsonSerializer.Deserialize<ProductStockUpdateDto>(result.Message.Value);
                        if (message != null)
                        {
                            // Aggiorna il database in base al messaggio
                            _productRepository.UpdateStockAsync(message.ProductId, message.NewStock).Wait();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Log dell'errore
                Console.WriteLine($"Errore durante l'elaborazione dei messaggi Kafka: {ex.Message}");
            }
            finally
            {
                consumer.Close();
            }
        }
    }
}
