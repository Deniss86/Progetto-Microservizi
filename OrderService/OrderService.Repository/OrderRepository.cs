using Microsoft.EntityFrameworkCore;
using OrderService.Repository.Abstraction;
using OrderService.Repository.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OrderService.Repository
{
    // Implementa il repository per la gestione degli ordini.
    public class OrderRepository : IOrderRepository
    {
        private readonly OrderDbContext _dbContext; // Istanza del contesto database

        // Costruttore con Dependency Injection del contesto database
        public OrderRepository(OrderDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        // Recupera un ordine tramite ID
        public async Task<Order> GetOrderByIdAsync(int id)
        {
            var order = await _dbContext.Orders.FindAsync(id);
            if (order == null)
            {
                throw new KeyNotFoundException($"Order with id {id} not found.");
            }
            return order;
        }

        // Recupera tutti gli ordini
        public async Task<IEnumerable<Order>> GetAllOrdersAsync()
        {
            return await _dbContext.Orders.ToListAsync();
        }

        // Aggiunge un nuovo ordine
        public async Task AddOrderAsync(Order order)
        {
            await _dbContext.Orders.AddAsync(order);
        }

        // Salva le modifiche nel database
        public async Task<bool> SaveChangesAsync()
        {
            return await _dbContext.SaveChangesAsync() > 0;
        }

        // Rimuove un ordine tramite ID
        public async Task RemoveAsync(int id)
        {
            var order = await GetOrderByIdAsync(id);
            if (order != null)
            {
                _dbContext.Orders.Remove(order);
            }
        }
        // Aggiorna un ordine esistente
        public async Task UpdateOrderStatusAsync(int id, string status)
        {
            var existingOrder = await _dbContext.Orders.FindAsync(id);
            if (existingOrder == null)
            {
                throw new KeyNotFoundException($"Order with id {id} not found.");
            }

            // Aggiorna solo lo stato
            existingOrder.Status = status;

            // Salva le modifiche
            await _dbContext.SaveChangesAsync();
        }


    }
}
