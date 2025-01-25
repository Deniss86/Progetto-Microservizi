using System.Threading.Tasks;

namespace OrderService.Kafka.Abstraction
{
    public interface IKafkaProducer
    {
        Task ProduceAsync<T>(string topic, T message);
    }
}
