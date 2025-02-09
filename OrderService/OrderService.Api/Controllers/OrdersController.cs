using Microsoft.AspNetCore.Mvc; // Importa il supporto per i controller API in ASP.NET Core
using OrderService.Business.Abstraction; // Importa l'interfaccia della logica di business degli ordini
//using OrderService.Kafka.Abstraction; // Importa l'interfaccia per il producer Kafka
using OrderService.ClientHttp.Abstraction; // Importa l'interfaccia per la comunicazione HTTP
using OrderService.Repository.Model; // Importa il modello degli ordini
using OrderService.Shared.Models; // Importa i modelli condivisi
using System.Collections.Generic; // Importa le collezioni generiche per gestire le eccezioni

namespace OrderService.Api.Controllers
{
    // Definisce il controller per la gestione degli ordini
    [Route("api/[controller]/[action]")]
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
        [HttpPost(Name = "CreateOrder")] // Routing

        // Metodo associato a CreateOrderAsync per creare un nuovo ordine di Business
        // Comunica con l'inventario con IClientHttp.UpdateStockAsync
        // Task indica che il metodo è asincrono e restituisce un IActionResult (HTTP Response)
        public async Task<IActionResult> CreateOrder([FromBody] Order order) // Action
        {
            await _orderBusiness.CreateOrderAsync(order); // Chiamata alla logica di business per creare l'ordine

            Console.WriteLine("Stock update request sent for ProductId: " + order.Quantity);
            // Invia un messaggio Kafka per notificare la creazione dell'ordine 
            //await _kafkaProducer.ProduceAsync("order-updates", order);

            // Restituisce HTTP 201 Created con il link all'ordine appena creato
            return CreatedAtAction(nameof(GetOrder), new { id = order.Id }, order);
        }

        // Endpoint HTTP GET per ottenere un ordine specifico tramite ID
        [HttpGet("{id}", Name = "GetOrderById")] // Routing
        public async Task<IActionResult> GetOrder(int id) // Action
        {
            var order = await _orderBusiness.GetOrderAsync(id); // Recupera l'ordine tramite la logica di business
            if (order == null)
                return NotFound(); // Restituisce HTTP 404 se l'ordine non esiste

            return Ok(order); // Restituisce HTTP 200 con l'ordine trovato
        }

        // Endpoint HTTP GET per ottenere tutti gli ordini
        [HttpGet(Name = "GetAllOrders")] // Routing

        // Metodo associato a GetAllOrdersAsync per ottenere tutti gli ordini di Business
        public async Task<IActionResult> GetAllOrders() // Action
        {
            var orders = await _orderBusiness.GetAllOrdersAsync(); // Recupera tutti gli ordini
            return Ok(orders); // Restituisce HTTP 200 con la lista degli ordini
        }

        // Endpoint HTTP DELETE per eliminare un ordine tramite ID
        // Metodo associato a DeleteOrderAsync per eliminare un ordine di Business
        [HttpDelete("{id}", Name = "DeleteOrder")] // Routing
        public async Task<IActionResult> DeleteOrder(int id)  // Action
        {
            try
            {
                await _orderBusiness.DeleteOrderAsync(id); // Chiama la logica di business per eliminare l'ordine
                return NoContent(); // Restituisce HTTP 204 No Content se l'eliminazione è avvenuta con successo
            }
            catch (KeyNotFoundException)
            {
                return NotFound(); // Restituisce HTTP 404 se l'ordine non esiste
            }
        }
        // Metodoo per aggiornare un ordine gia esistente
        // Si appoggia a UpdateOrderAsync della Business
       [HttpPut("{id}/status", Name = "UpdateOrderStatus")] // Routing
        public async Task<IActionResult> UpdateOrderStatus(int id, [FromBody] string status) // Action
        {
            try
            {
                await _orderBusiness.UpdateOrderStatusAsync(id, status); // Chiama il metodo di business per aggiornare lo stato
                return NoContent(); // HTTP 204
            }
            catch (KeyNotFoundException)
            {
                return NotFound(); // HTTP 404 se l'ordine non esiste
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message); // HTTP 400 per eventuali errori
            }
        }
    }
}
