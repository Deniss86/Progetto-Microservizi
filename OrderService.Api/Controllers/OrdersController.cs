using Microsoft.AspNetCore.Mvc;
using OrderService.Repository;
using OrderService.Shared.Models;

namespace OrderService.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly OrderDbContext _dbContext;

        public OrdersController(OrderDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        // POST: api/orders
        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] Order order)
        {
            _dbContext.Orders.Add(order);
            await _dbContext.SaveChangesAsync();
            return CreatedAtAction(nameof(GetOrder), new { id = order.Id }, order);
        }

        // GET: api/orders/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrder(int id)
        {
            var order = await _dbContext.Orders.FindAsync(id);
            if (order == null)
                return NotFound();
            return Ok(order);
        }
    }
}
