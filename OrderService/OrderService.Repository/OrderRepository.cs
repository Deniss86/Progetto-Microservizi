using Microsoft.EntityFrameworkCore; // Importa Entity Framework Core per le operazioni sul database
using OrderService.Repository.Abstraction; // Importa l'interfaccia del repository
using OrderService.Repository.Model; // Importa il modello Order
using System.Collections.Generic; // Importa le collezioni generiche
using System.Threading.Tasks; // Importa il supporto per operazioni asincrone

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
            var order = await _dbContext.Orders.FindAsync(id); // Cerca l'ordine per ID
            // Se non esiste, lancia un'eccezione
            if (order == null)
            {
                // Lancia un'eccezione con messaggio personalizzato 
                throw new KeyNotFoundException($"Order with id {id} not found.");
            }
            return order; // Restituisce l'ordine trovato
        }

        // Recupera tutti gli ordini
        public async Task<IEnumerable<Order>> GetAllOrdersAsync()
        {
            return await _dbContext.Orders.ToListAsync(); // Restituisce tutti gli ordini come lista
        }

        // Aggiunge un nuovo ordine
        public async Task AddOrderAsync(Order order)
        {
            await _dbContext.Orders.AddAsync(order); // Aggiunge l'ordine al Db
        }

        // Salva le modifiche nel database
        public async Task<bool> SaveChangesAsync()
        {
            return await _dbContext.SaveChangesAsync() > 0; // Ritorna true se almeno una modifica è stata salvata
        }

        // Rimuove un ordine tramite ID
        public async Task RemoveAsync(int id)
        {
            var order = await GetOrderByIdAsync(id); // Cerca l'ordine nel database

            if (order != null)
            {
                _dbContext.Orders.Remove(order); // Se l'ordine esiste, lo rimuove
            }
        }
        // Aggiorna un ordine esistente
        public async Task UpdateOrderStatusAsync(int id, string status)
        {
            var existingOrder = await _dbContext.Orders.FindAsync(id); // Cerca l'ordine nel database
            if (existingOrder == null)
            {
                throw new KeyNotFoundException($"Order with id {id} not found."); // Se l'ordine non esiste, lancia un'eccezione
            } 

            // Aggiorna solo lo stato
            existingOrder.Status = status;

            // Salva le modifiche
            await _dbContext.SaveChangesAsync();
        }


    }
}
