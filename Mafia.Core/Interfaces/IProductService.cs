using Mafia.Core.Models;

namespace Mafia.Core.Interfaces
{
    public interface IProductService
    {
        Task<IEnumerable<Product>> GetAllProductsAsync();
        Task<IEnumerable<Product>> GetProductsByCategoryAsync(string category);
        Task<Product?> GetProductByIdAsync(Guid id);
        Task<Guid> CreateProductAsync(Product product);
        Task UpdateProductAsync(Product product);
        Task DeleteProductAsync(Guid id);
        Task<bool> UpdateProductStockAsync(Guid id, int quantity);
    }
} 