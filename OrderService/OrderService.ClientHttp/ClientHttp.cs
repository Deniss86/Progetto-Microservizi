using System.Net.Http; // Importa la classe HttpClient per effettuare richieste HTTP
using System.Net.Http.Json; // Importa i metodi helper per la serializzazione JSON
using System.Threading; // Importa la gestione dei token di cancellazione
using System.Threading.Tasks; // Importa il supporto per le operazioni asincrone
using OrderService.ClientHttp.Abstraction; // Importa l'interfaccia per la comunicazione HTTP
using OrderService.Shared.Models; // Importa i DTO condivisi

namespace OrderService.ClientHttp
{
    // Implementa l'interfaccia IClientHttp per la comunicazione con servizi remoti
    public class ClientHttp : IClientHttp
    {
        private readonly HttpClient _httpClient; // Istanza di HttpClient per effettuare richieste HTTP

        // Costruttore che riceve un'istanza di HttpClient tramite Dependency Injection
        public ClientHttp(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        // Metodo per recuperare un prodotto dal servizio remoto tramite il suo ID
        public async Task<ProductDto?> GetProductByIdAsync(int productId, CancellationToken cancellationToken = default)
        {
            // Effettua una richiesta GET per ottenere i dettagli del prodotto dal servizio InventoryService
            return await _httpClient.GetFromJsonAsync<ProductDto>($"api/products/{productId}", cancellationToken);
        }

        // Metodo per aggiornare lo stock di un prodotto tramite una richiesta HTTP POST
        public async Task UpdateStockAsync(ProductStockUpdateDto stockUpdate, CancellationToken cancellationToken = default)
        {
            try
            {
                Console.WriteLine($"[DEBUG] OrderService sta inviando la richiesta a InventoryService: ProductId = {stockUpdate.ProductId}, Quantity = {stockUpdate.Quantity}");
                var response = await _httpClient.PostAsJsonAsync("/api/Products/UpdateStock/update-stock", stockUpdate, cancellationToken);
                
                if (!response.IsSuccessStatusCode)
                {
                    var errorMessage = await response.Content.ReadAsStringAsync();
                    throw new Exception($"Inventory update failed: {errorMessage}");
                }
            }
            catch (HttpRequestException ex)
            {
                throw new Exception("Failed to communicate with InventoryService.", ex);
            }
        }
    }
}
