using InventoryService.Repository.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InventoryService.Repository.Abstraction
{
    public interface IProductRepository
    {
        Task<Product> GetProductByIdAsync(int id);
        Task<IEnumerable<Product>> GetAllProductsAsync();
        Task AddProductAsync(Product product);
        Task<bool> SaveChangesAsync();
        Task RemoveAsync(int id);
    }
}
