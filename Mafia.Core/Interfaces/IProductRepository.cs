using Mafia.Core.Models;

namespace Mafia.Core.Interfaces
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetAllAsync();
        Task<IEnumerable<Product>> GetByCategoryAsync(string category);
        Task<Product?> GetByIdAsync(string id);
        Task<string> CreateAsync(Product product);
        Task UpdateAsync(Product product);
        Task DeleteAsync(string id);
        Task<bool> UpdateStockAsync(string id, int quantity);
    }
} 