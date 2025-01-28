using System.Threading.Tasks;

namespace InventoryService.Kafka.Abstraction
{
    public interface IKafkaProducer
    {
        Task ProduceAsync<T>(string topic, T message);
    }
}
