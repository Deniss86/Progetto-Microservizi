using Microsoft.AspNetCore.Mvc;
using OrderService.Business.Abstraction;
using OrderService.Kafka.Abstraction;
using OrderService.Shared.Models;

namespace OrderService.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderBusiness _orderBusiness;
        private readonly IKafkaProducer _kafkaProducer;

        public OrdersController(IOrderBusiness orderBusiness, IKafkaProducer kafkaProducer)
        {
            _orderBusiness = orderBusiness;
            _kafkaProducer = kafkaProducer;
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] Order order)
        {
            await _orderBusiness.CreateOrderAsync(order);

            // Invia un messaggio al topic Kafka
            await _kafkaProducer.ProduceAsync("order-updates", order);

            return CreatedAtAction(nameof(GetOrder), new { id = order.Id }, order);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrder(int id)
        {
            var order = await _orderBusiness.GetOrderAsync(id);
            if (order == null)
                return NotFound();

            return Ok(order);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllOrders()
        {
            var orders = await _orderBusiness.GetAllOrdersAsync();
            return Ok(orders);
        }
    }
}
