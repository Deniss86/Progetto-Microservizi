/*
using System.Text.Json; // Importa il supporto per la serializzazione JSON
using Confluent.Kafka; // Importa la libreria Kafka per .NET
using InventoryService.Repository.Abstraction; // Importa l'interfaccia del repository prodotti
using InventoryService.Shared.DTOs; // Importa i Data Transfer Objects (DTO)

namespace InventoryService.Kafka
{
    // Implementazione del consumer Kafka
    public class KafkaConsumer : IKafkaConsumer
    {
        private readonly string _bootstrapServers; // Indirizzo dei server Kafka
        private readonly string _groupId; // ID del gruppo consumer
        private readonly IProductRepository _productRepository; // Repository per la gestione dei prodotti

        // Costruttore che inizializza i parametri di connessione e il repository
        public KafkaConsumer(string bootstrapServers, string groupId, IProductRepository productRepository)
        {
            _bootstrapServers = bootstrapServers;
            _groupId = groupId;
            _productRepository = productRepository;
        }

        // Metodo per consumare messaggi da Kafka
        public void Consume(string topic, CancellationToken cancellationToken)
        {
            // Configurazione del consumer Kafka
            var config = new ConsumerConfig
            {
                BootstrapServers = _bootstrapServers,
                GroupId = _groupId,
                AutoOffsetReset = AutoOffsetReset.Earliest // Inizia a leggere i messaggi meno recenti se nessun offset è stato memorizzato
            };

            // Crea il consumer Kafka
            using var consumer = new ConsumerBuilder<Ignore, string>(config).Build();
            consumer.Subscribe(topic); // Sottoscrive al topic specificato

            try
            {
                while (!cancellationToken.IsCancellationRequested) // Continua a leggere finché non viene annullato
                {
                    var result = consumer.Consume(cancellationToken); // Consuma il messaggio

                    if (result != null)
                    {
                        // Deserializza il messaggio JSON ricevuto
                        var message = JsonSerializer.Deserialize<ProductStockUpdateDto>(result.Message.Value);
                        if (message != null)
                        {
                            // Aggiorna lo stock del prodotto nel database
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
                consumer.Close(); // Chiude il consumer Kafka in modo sicuro
            }
        }
    }
}
