using Mafia.Core.Models;
using Microsoft.AspNetCore.Http;
namespace Mafia.Core.Interfaces
{
    public interface IProductService
    {
        Task<IEnumerable<Product>> GetAllProductsAsync();
        Task<Product?> GetProductByIdAsync(string id);
        Task<string> CreateProductAsync(string name, string description, double price, int availableQuantity, string category, IFormFile image);
        Task<Product?> UpdateProductAsync(string id, string name, string description, double? price, int? availableQuantity, string category, IFormFile image);
        Task<Product?> DeleteProductAsync(string id);
        Task<Product?> UpdateProductStockAsync(string id, int quantity);
    }
} 