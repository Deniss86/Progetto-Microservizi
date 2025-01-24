using OrderService.Shared.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OrderService.Repository.Abstraction
{
    public interface IOrderRepository
    {
        Task<Order> GetOrderByIdAsync(int id);
        Task<IEnumerable<Order>> GetAllOrdersAsync();
        Task AddOrderAsync(Order order);
        Task<bool> SaveChangesAsync();
        Task RemoveAsync(int id);
    }
}
