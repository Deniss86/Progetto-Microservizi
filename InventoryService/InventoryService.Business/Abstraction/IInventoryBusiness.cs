using InventoryService.Shared.DTOs;

namespace InventoryService.Business.Abstraction
{
    public interface IInventoryBusiness
    {
        Task<IEnumerable<ProductDto>> GetAllProductsAsync();
        Task<ProductDto?> GetProductByIdAsync(int id);
        Task UpdateStockAsync(int productId, int quantity);
        Task AddProductAsync(ProductDto productDto);
    }
}
