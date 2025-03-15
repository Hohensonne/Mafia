using Mafia.Core.Models;

namespace Mafia.Core.Interfaces
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetAllAsync();
        Task<IEnumerable<Product>> GetByCategoryAsync(string category);
        Task<Product?> GetByIdAsync(string id);
        Task<string> CreateAsync(Product product);
        Task<Product?> UpdateAsync(Product product);
        Task<Product?> DeleteAsync(string id);
        Task<Product?> UpdateStockAsync(string id, int quantity);
    }
} 