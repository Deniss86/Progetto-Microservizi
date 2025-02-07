using System.Threading; // Importa la gestione dei token di cancellazione per le richieste asincrone
using System.Threading.Tasks; // Importa il supporto per le operazioni asincrone
using OrderService.Shared.Models; // Importa i modelli condivisi dell'OrderService

namespace OrderService.ClientHttp.Abstraction
{
    // Definisce un'interfaccia per la comunicazione HTTP con altri servizi
    public interface IClientHttp
    {
        // Metodo per ottenere un prodotto dal servizio remoto in base all'ID
        Task<ProductDto?> GetProductByIdAsync(int productId, CancellationToken cancellationToken = default);
        // CancellationToken → Permette di interrompere le richieste HTTP in caso di timeout o altre esigenze.
        
        // Metodo per aggiornare lo stock di un prodotto tramite una richiesta HTTP POST
        Task UpdateStockAsync(ProductStockUpdateDto stockUpdate, CancellationToken cancellationToken = default);
    }
}
