using OrderService.Shared.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OrderService.Business.Abstraction
{
    public interface IOrderBusiness
    {
        Task CreateOrderAsync(Order order);
        Task<Order?> GetOrderAsync(int id);
        Task<IEnumerable<Order>> GetAllOrdersAsync();
        Task UpdateOrderAsync(int id, Order order);
        Task DeleteOrderAsync(int id);
    }
}
