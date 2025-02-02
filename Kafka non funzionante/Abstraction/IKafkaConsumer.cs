/*
using System;
using System.Threading;
using System.Threading.Tasks;

namespace InventoryService.Kafka.Abstraction
{
    // Interfaccia per la gestione dei Consumer Kafka
    public interface IKafkaConsumer
    {
        // Metodo per sottoscriversi a un topic specifico
        void Subscribe(string topic);

        // Metodo per consumare messaggi da un topic Kafka e invocarne l'elaborazione
        void Consume(Func<string, Task> onMessageReceived, CancellationToken cancellationToken);
    }
}
