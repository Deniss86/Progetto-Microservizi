using OrderService.Business.Abstraction; // Importa l'interfaccia della logica di business
using OrderService.Repository.Abstraction; // Importa l'interfaccia del repository degli ordini
using OrderService.Repository.Model; // Importa il modello degli ordini
using OrderService.ClientHttp.Abstraction; // Importa l'interfaccia per la comunicazione HTTP
using OrderService.Shared.Models; // Importa i modelli condivisi
using System.Text.Json; // Importa il supporto per la serializzazione JSON
 
namespace OrderService.Business
{
    // Implementa la logica di business per la gestione degli ordini
    public class OrderBusiness : IOrderBusiness
    {
        private readonly IOrderRepository _orderRepository; // Istanza del repository degli ordini
        private readonly IClientHttp _clientHttp; // Istanza del client HTTP
        //private readonly IKafkaProducer _kafkaProducer; // Istanza del producer Kafka

        // Costruttore che riceve il repository degli ordini e il client HTTP tramite Dependency Injection
        public OrderBusiness(IOrderRepository orderRepository, IClientHttp clientHttp) // Rimosso Kafka
        {
            _orderRepository = orderRepository;
            _clientHttp = clientHttp;
            //_kafkaProducer = kafkaProducer;
        }

        // Metodo per creare un nuovo ordine
        public async Task CreateOrderAsync(Order order)
        {
            if (order.Quantity <= 0)
                throw new ArgumentException("Invalid order quantity");

            try
            {
                // Invia una richiesta HTTP al servizio di inventario per aggiornare lo stock PRIMA di salvare l'ordine
                var stockUpdate = new ProductStockUpdateDto
                {
                    ProductId = order.ProductId,
                    Quantity = order.Quantity
                };

                await _clientHttp.UpdateStockAsync(stockUpdate);

                // Se l'aggiornamento dello stock è riuscito, allora salva l'ordine nel database
                await _orderRepository.AddOrderAsync(order);
                await _orderRepository.SaveChangesAsync();
            }
            catch (HttpRequestException ex)
            {
                // Se la richiesta al servizio Inventory fallisce, l'ordine non viene salvato
                throw new Exception("Failed to update stock in InventoryService. Order creation aborted.", ex);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while creating the order.", ex);
            }
        }


        // Metodo per ottenere un ordine tramite ID
        public async Task<Order?> GetOrderAsync(int id)
        {
            return await _orderRepository.GetOrderByIdAsync(id); // Recupera l'ordine dal database
        }

        // Metodo per ottenere tutti gli ordini
        public async Task<IEnumerable<Order>> GetAllOrdersAsync()
        {
            return await _orderRepository.GetAllOrdersAsync(); // Recupera tutti gli ordini
        }
        // Metodo per aggiornare lo stato di un ordine 
        public async Task UpdateOrderStatusAsync(int id, string status)
        {
            var existingOrder = await _orderRepository.GetOrderByIdAsync(id);
            if (existingOrder == null)
            {
                throw new KeyNotFoundException("Order not found");
            }

            // Aggiorna solo lo stato
            existingOrder.Status = status;

            // Salva le modifiche
            await _orderRepository.UpdateOrderStatusAsync(id, status);
        }

        // Metodo per cancellare un ordine
        public async Task DeleteOrderAsync(int id)
        {
            await _orderRepository.RemoveAsync(id);
            await _orderRepository.SaveChangesAsync();
        }
    }
}
