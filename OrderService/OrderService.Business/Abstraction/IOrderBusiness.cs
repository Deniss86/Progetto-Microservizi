using OrderService.Shared.Models; // Importa i modelli condivisi degli ordini
using System.Collections.Generic; // Importa il supporto per le collezioni
using System.Threading.Tasks; // Importa il supporto per le operazioni asincrone

namespace OrderService.Business.Abstraction
{
    // Definisce un'interfaccia per la gestione della logica di business degli ordini
    public interface IOrderBusiness
    {
        // Metodo per creare un nuovo ordine
        Task CreateOrderAsync(Order order);

        // Metodo per ottenere un ordine tramite ID
        Task<Order?> GetOrderAsync(int id);

        // Metodo per ottenere tutti gli ordini
        Task<IEnumerable<Order>> GetAllOrdersAsync();

        // Metodo per aggiornare un ordine esistente
        Task UpdateOrderAsync(int id, Order order);

        // Metodo per eliminare un ordine
        Task DeleteOrderAsync(int id);
    }
}
