namespace OrderService.Shared.Models
{
    // DTO per rappresentare una richiesta di aggiornamento dello stock
    // Questo DTO è usato in ClientHttp.cs per inviare una richiesta POST al servizio di inventario.
    //Viene consumato da KafkaConsumer.cs per aggiornare lo stock in base ai messaggi ricevuti da Kafka.
    public class ProductStockUpdateDto
    {
        public int ProductId { get; set; } // ID del prodotto da aggiornare
        public int Quantity { get; set; } // Quantità da aggiungere o rimuovere (positivo/negativo)
    }
}
