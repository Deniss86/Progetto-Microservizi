using Microsoft.EntityFrameworkCore;
using OrderService.Repository.Abstraction;
using OrderService.Shared.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OrderService.Repository
{
    public class OrderRepository : IOrderRepository
    {
        private readonly OrderDbContext _dbContext;

        public OrderRepository(OrderDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Order> GetOrderByIdAsync(int id)
        {
            var order = await _dbContext.Orders.FindAsync(id);
            if (order == null)
            {
                throw new KeyNotFoundException($"Order with id {id} not found.");
            }
            return order;
        }

        public async Task<IEnumerable<Order>> GetAllOrdersAsync()
        {
            return await _dbContext.Orders.ToListAsync();
        }

        public async Task AddOrderAsync(Order order)
        {
            await _dbContext.Orders.AddAsync(order);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await _dbContext.SaveChangesAsync() > 0;
        }

        public async Task RemoveAsync(int id)
        {
            var order = await GetOrderByIdAsync(id);
            if (order != null)
            {
                _dbContext.Orders.Remove(order);
            }
        }
    }
}
