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
            _httpClient = httpClient; // Assegna l'istanza di HttpClient ricevuta al campo 
        }

        // Metodo per recuperare un prodotto dal servizio remoto tramite il suo ID
        public async Task<ProductDto?> GetProductByIdAsync(int productId, CancellationToken cancellationToken = default)
        {
            // Effettua una richiesta GET per ottenere i dettagli del prodotto dal servizio InventoryService
            return await _httpClient.GetFromJsonAsync<ProductDto>($"api/products/{productId}", cancellationToken);
        }

        // Metodo per aggiornare lo stock di un prodotto tramite una richiesta HTTP POST a InventoryService
        public async Task UpdateStockAsync(ProductStockUpdateDto stockUpdate, CancellationToken cancellationToken = default)
        {
            try
            {   // Log di debug per tracciare le richieste inviate al servizio InventoryService
                Console.WriteLine($"[DEBUG] OrderService sta inviando la richiesta a InventoryService: ProductId = {stockUpdate.ProductId}, Quantity = {stockUpdate.Quantity}");
                
                //Effettua una richiesta HTTP POST all'endpoint del servizio di inventario
                var response = await _httpClient.PostAsJsonAsync("/api/Products/UpdateStock/update-stock", stockUpdate, cancellationToken);
                
                // Controlla se la risposta HTTP ha avuto successo, altrimenti genera un'eccezione
                if (!response.IsSuccessStatusCode)
                {
                    var errorMessage = await response.Content.ReadAsStringAsync(); // Ottiene il messaggio di errore dalla risposta
                    throw new Exception($"Inventory update failed: {errorMessage}"); // Lancia un'eccezione con il messaggio di errore
                }
            }
            catch (HttpRequestException ex)
            {
                // Se la comunicazione con InventoryService fallisce, genera un'eccezione personalizzata
                throw new Exception("Failed to communicate with InventoryService.", ex);
            }
        }
    }
}
