using Mafia.Core.Models;

namespace Mafia.Core.Interfaces
{
    public interface IProductService
    {
        Task<IEnumerable<Product>> GetAllProductsAsync();
        Task<IEnumerable<Product>> GetProductsByCategoryAsync(string category);
        Task<Product?> GetProductByIdAsync(string id);
        Task<string> CreateProductAsync(Product product);
        Task UpdateProductAsync(Product product);
        Task DeleteProductAsync(string id);
        Task<bool> UpdateProductStockAsync(string id, int quantity);
    }
} 