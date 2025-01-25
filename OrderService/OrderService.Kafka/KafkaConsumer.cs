using Confluent.Kafka;
using System;
using System.Threading;

namespace OrderService.Kafka
{
    public class KafkaConsumer
    {
        private readonly string _topic;
        private readonly IConsumer<Null, string> _consumer;

        public KafkaConsumer(string bootstrapServers, string groupId, string topic)
        {
            _topic = topic;

            var config = new ConsumerConfig
            {
                BootstrapServers = bootstrapServers,
                GroupId = groupId,
                AutoOffsetReset = AutoOffsetReset.Earliest
            };

            _consumer = new ConsumerBuilder<Null, string>(config).Build();
        }

        public void Consume(CancellationToken cancellationToken)
        {
            _consumer.Subscribe(_topic);

            try
            {
                while (!cancellationToken.IsCancellationRequested)
                {
                    var result = _consumer.Consume(cancellationToken);
                    Console.WriteLine($"Message received from topic {_topic}: {result.Value}");
                }
            }
            catch (OperationCanceledException)
            {
                _consumer.Close();
            }
        }
    }
}
