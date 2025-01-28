using Confluent.Kafka;
using InventoryService.Kafka.Abstraction;

namespace InventoryService.Kafka
{
    public class KafkaConsumer : IKafkaConsumer
    {
        private readonly IConsumer<Null, string> _consumer;

        public KafkaConsumer(string bootstrapServers, string groupId)
        {
            var config = new ConsumerConfig
            {
                BootstrapServers = bootstrapServers,
                GroupId = groupId,
                AutoOffsetReset = AutoOffsetReset.Earliest
            };
            _consumer = new ConsumerBuilder<Null, string>(config).Build();
        }

        public void Subscribe(string topic)
        {
            _consumer.Subscribe(topic);
        }

        public void Consume(Func<string, Task> onMessageReceived, CancellationToken cancellationToken)
        {
            Task.Run(() =>
            {
                while (!cancellationToken.IsCancellationRequested)
                {
                    var result = _consumer.Consume(cancellationToken);
                    onMessageReceived(result.Message.Value);
                }
            }, cancellationToken);
        }
    }
}
