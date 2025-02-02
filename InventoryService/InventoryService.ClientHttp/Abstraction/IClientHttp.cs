using InventoryService.Shared.DTOs; // Importa i Data Transfer Objects (DTO)
using System.Threading; // Importa la gestione dei token di cancellazione per le richieste asincrone
using System.Threading.Tasks; // Importa il supporto per le operazioni asincrone

namespace InventoryService.ClientHttp.Abstraction
{
    // Definisce un'interfaccia per la comunicazione HTTP con altri servizi
    public interface IClientHttp
    {
        // Metodo per ottenere un prodotto dal servizio remoto in base all'ID
        Task<ProductDto?> GetProductByIdAsync(int productId, CancellationToken cancellationToken = default);

        // Metodo per aggiornare lo stock di un prodotto tramite una richiesta HTTP POST
        Task<bool> UpdateStockAsync(ProductStockUpdateDto stockUpdate, CancellationToken cancellationToken = default);
    }
}
