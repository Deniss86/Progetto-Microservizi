using System.Threading;
using System.Threading.Tasks;
using OrderService.Shared.Models;

namespace OrderService.ClientHttp.Abstraction
{
    public interface IClientHttp
    {
        Task<ProductDto?> GetProductByIdAsync(int productId, CancellationToken cancellationToken = default);
        Task<bool> UpdateStockAsync(ProductStockUpdateDto stockUpdate, CancellationToken cancellationToken = default);
    }
}
