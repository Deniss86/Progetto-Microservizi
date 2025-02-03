using OrderService.Shared.Models; // Importa il modello degli ordini
using System.Collections.Generic; // Importa il supporto per le collezioni
using System.Threading.Tasks; // Importa il supporto per le operazioni asincrone

namespace OrderService.Repository.Abstraction
{
    // Definisce un'interfaccia per la gestione del repository degli ordini
    public interface IOrderRepository
    {
        // Metodo per ottenere un ordine tramite ID
        Task<Order> GetOrderByIdAsync(int id);

        // Metodo per ottenere tutti gli ordini
        Task<IEnumerable<Order>> GetAllOrdersAsync();

        // Metodo per aggiungere un nuovo ordine
        Task AddOrderAsync(Order order);

        // Metodo per salvare le modifiche nel database
        Task<bool> SaveChangesAsync();

        // Metodo per rimuovere un ordine tramite ID
        Task RemoveAsync(int id);

        // Metodo per aggiornare un ordine esistente
        Task UpdateOrderStatusAsync(int id, string status);

    }
}
