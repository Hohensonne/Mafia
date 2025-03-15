using System.Reflection.Metadata.Ecma335;
using Mafia.Core.Interfaces;
using Mafia.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace Mafia.Persistence.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly ApplicationDbContext _context;

        public ProductRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            return await _context.Products.ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetByCategoryAsync(string category)
        {
            return await _context.Products
                .Where(p => p.Category == category)
                .ToListAsync();
        }

        public async Task<Product?> GetByIdAsync(string id)
        {
            return await _context.Products.FindAsync(id);
        }

        public async Task<string> CreateAsync(Product product)
        {
            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();
            return product.Id;
        }

        public async Task<Product?> UpdateAsync(Product product)
        {
            _context.Products.Update(product);
            await _context.SaveChangesAsync();
            return product;
        }

        public async Task<Product?> DeleteAsync(string id)
        {
            var product = await _context.Products.FindAsync(id) ?? throw new Exception("Product not found");
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return product;
        }

        public async Task<Product?> UpdateStockAsync(string id, int quantity)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
               throw new Exception("Product not found");
            }

            if (quantity < 0)
            {
                throw new Exception("Invalid quantity");
            }

            product.AvailableQuantity = quantity;
            await _context.SaveChangesAsync();
            return product;
        }
    }
} 