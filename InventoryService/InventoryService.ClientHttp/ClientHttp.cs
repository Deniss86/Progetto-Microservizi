using InventoryService.ClientHttp.Abstraction;
using InventoryService.Shared.DTOs;
using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;

namespace InventoryService.ClientHttp
{
    public class ClientHttp : IClientHttp
    {
        private readonly HttpClient _httpClient;

        public ClientHttp(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<ProductDto?> GetProductByIdAsync(int productId, CancellationToken cancellationToken = default)
        {
            try
            {
                return await _httpClient.GetFromJsonAsync<ProductDto>($"api/products/{productId}", cancellationToken);
            }
            catch (Exception)
            {
                // Log the exception if necessary
                return null;
            }
        }
        
        public async Task<bool> UpdateStockAsync(ProductStockUpdateDto stockUpdate, CancellationToken cancellationToken = default)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("api/products/update-stock", stockUpdate, cancellationToken);
                return response.IsSuccessStatusCode;
            }
            catch (Exception)
            {
                // Log the exception if necessary
                return false;
            }
        }
    }
}
