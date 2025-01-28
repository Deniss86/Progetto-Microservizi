using System;
using System.Threading;
using System.Threading.Tasks;

namespace InventoryService.Kafka.Abstraction
{
    public interface IKafkaConsumer
    {
        void Subscribe(string topic);
        void Consume(Func<string, Task> onMessageReceived, CancellationToken cancellationToken);
    }
}
