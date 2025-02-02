using Microsoft.AspNetCore.Mvc; // Importa il supporto per i controller API in ASP.NET Core
using OrderService.Business.Abstraction; // Importa l'interfaccia della logica di business degli ordini
//using OrderService.Kafka.Abstraction; // Importa l'interfaccia per il producer Kafka
using OrderService.ClientHttp.Abstraction; // Importa l'interfaccia per la comunicazione HTTP
using OrderService.Shared.Models; // Importa il modello Order

namespace OrderService.Api.Controllers
{
    // Definisce il controller per la gestione degli ordini
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderBusiness _orderBusiness; // Istanza della logica di business per gli ordini
        private readonly IClientHttp _clientHttp; // Istanza del client HTTP
        //private readonly IKafkaProducer _kafkaProducer; // Istanza del producer Kafka

        // Costruttore che riceve la logica di business e il client HTTP tramite Dependency Injection
        public OrdersController(IOrderBusiness orderBusiness, IClientHttp clientHttp) // Rimosso Kafka
        {
            _orderBusiness = orderBusiness;
            _clientHttp = clientHttp;
            //_kafkaProducer = kafkaProducer;
        }

        // Endpoint HTTP POST per creare un nuovo ordine
        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] Order order)
        {
            await _orderBusiness.CreateOrderAsync(order); // Chiamata alla logica di business per creare l'ordine

            // Invia una richiesta HTTP al servizio di inventario per aggiornare lo stock
            var stockUpdate = new ProductStockUpdateDto
            {
                ProductId = order.ProductId,
                Quantity = order.Quantity
            };
            await _clientHttp.UpdateStockAsync(stockUpdate);

            // Invia un messaggio Kafka per notificare la creazione dell'ordine (commentato)
            //await _kafkaProducer.ProduceAsync("order-updates", order);

            // Restituisce HTTP 201 Created con il link all'ordine appena creato
            return CreatedAtAction(nameof(GetOrder), new { id = order.Id }, order);
        }

        // Endpoint HTTP GET per ottenere un ordine specifico tramite ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrder(int id)
        {
            var order = await _orderBusiness.GetOrderAsync(id); // Recupera l'ordine tramite la logica di business
            if (order == null)
                return NotFound(); // Restituisce HTTP 404 se l'ordine non esiste

            return Ok(order); // Restituisce HTTP 200 con l'ordine trovato
        }

        // Endpoint HTTP GET per ottenere tutti gli ordini
        [HttpGet]
        public async Task<IActionResult> GetAllOrders()
        {
            var orders = await _orderBusiness.GetAllOrdersAsync(); // Recupera tutti gli ordini
            return Ok(orders); // Restituisce HTTP 200 con la lista degli ordini
        }
    }
}
