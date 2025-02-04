using InventoryService.Shared.DTOs; // Importa i Data Transfer Objects (DTOs)

namespace InventoryService.Business.Abstraction
{
    // Definisce un'interfaccia per la gestione dell'inventario
    public interface IInventoryBusiness
    {
        // Metodo per ottenere tutti i prodotti disponibili
        Task<IEnumerable<ProductDto>> GetAllProductsAsync();

        // Metodo per ottenere un prodotto specifico tramite il suo ID
        Task<ProductDto?> GetProductByIdAsync(int id);

        // Metodo per aggiornare la quantità di stock di un prodotto
        Task UpdateStockAsync(int productId, int quantity);

        // Metodo per aggiungere un nuovo prodotto all'inventario
        Task AddProductAsync(ProductDto productDto);
        
        // Metodo per rimuovere un prodotto dall'inventario
        Task RemoveProductAsync(int id);
    }
}
