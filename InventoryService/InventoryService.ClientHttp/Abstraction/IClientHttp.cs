using InventoryService.Shared.DTOs;
using System.Threading;
using System.Threading.Tasks;

namespace InventoryService.ClientHttp.Abstraction
{
    public interface IClientHttp
    {
        Task<ProductDto?> GetProductByIdAsync(int productId, CancellationToken cancellationToken = default);
        Task<bool> UpdateStockAsync(ProductStockUpdateDto stockUpdate, CancellationToken cancellationToken = default);
    }
}
