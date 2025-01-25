using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;
using OrderService.ClientHttp.Abstraction;
using OrderService.Shared.Models;

namespace OrderService.ClientHttp
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
            return await _httpClient.GetFromJsonAsync<ProductDto>($"api/products/{productId}", cancellationToken);
        }

        public async Task<bool> UpdateStockAsync(ProductStockUpdateDto stockUpdate, CancellationToken cancellationToken = default)
        {
            var response = await _httpClient.PostAsJsonAsync("api/stock/update", stockUpdate, cancellationToken);
            return response.IsSuccessStatusCode;
        }
    }
}
