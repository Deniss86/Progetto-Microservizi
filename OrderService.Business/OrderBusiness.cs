using OrderService.Business.Abstraction;
using OrderService.Repository.Abstraction;
using OrderService.Shared.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OrderService.Business
{
    public class OrderBusiness : IOrderBusiness
    {
        private readonly IOrderRepository _orderRepository;

        public OrderBusiness(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task CreateOrderAsync(Order order)
        {
            if (order.Quantity <= 0)
                throw new ArgumentException("Invalid order quantity");

            await _orderRepository.AddOrderAsync(order);
            await _orderRepository.SaveChangesAsync();
        }

        public async Task<Order?> GetOrderAsync(int id)
        {
            return await _orderRepository.GetOrderByIdAsync(id);
        }

        public async Task<IEnumerable<Order>> GetAllOrdersAsync()
        {
            return await _orderRepository.GetAllOrdersAsync();
        }

        public async Task UpdateOrderAsync(int id, Order order)
        {
            var existingOrder = await _orderRepository.GetOrderByIdAsync(id);
            if (existingOrder == null)
                throw new KeyNotFoundException("Order not found");

            existingOrder.Quantity = order.Quantity;
            existingOrder.TotalPrice = order.TotalPrice;
            existingOrder.Status = order.Status;

            await _orderRepository.SaveChangesAsync();
        }

        public async Task DeleteOrderAsync(int id)
        {
            await _orderRepository.RemoveAsync(id);
            await _orderRepository.SaveChangesAsync();
        }
    }
}
