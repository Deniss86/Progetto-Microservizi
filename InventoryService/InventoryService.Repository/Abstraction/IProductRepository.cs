using InventoryService.Repository.Model; // Importa il modello Product
using System.Collections.Generic; // Importa il supporto per le collezioni
using System.Threading.Tasks; // Importa il supporto per le operazioni asincrone

namespace InventoryService.Repository.Abstraction
{
    // Definisce un'interfaccia per il repository dei prodotti
    public interface IProductRepository
    {
        // Metodo per ottenere un prodotto tramite ID
        Task<Product> GetProductByIdAsync(int id);

        // Metodo per ottenere tutti i prodotti disponibili
        Task<IEnumerable<Product>> GetAllProductsAsync();

        // Metodo per aggiungere un nuovo prodotto
        Task AddProductAsync(Product product);

        // Metodo per salvare le modifiche nel database
        Task<bool> SaveChangesAsync();

        // Metodo per rimuovere un prodotto dal database tramite ID
        Task RemoveAsync(int id);
    }
}
