/*
using System.Threading.Tasks;

namespace InventoryService.Kafka.Abstraction
{
    // Interfaccia per la gestione dei Producer Kafka
    public interface IKafkaProducer
    {
        // Metodo per produrre e inviare un messaggio su un topic Kafka
        Task ProduceAsync<T>(string topic, T message);
    }
}
