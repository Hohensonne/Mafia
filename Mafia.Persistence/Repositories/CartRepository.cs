using Mafia.Core.Interfaces;
using Mafia.Core.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace Mafia.Persistence.Repositories
{
    public class CartRepository : ICartRepository
    {
        private readonly ApplicationDbContext _context;

        public CartRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Cart>> GetAllByUserIdAsync(string userId)
        {
            return await _context.Carts
                .Include(c => c.Product)
                .Select(c => new Cart
                {
                    Id = c.Id,
                    UserId = c.UserId,
                    ProductId = c.ProductId,
                    Quantity = c.Quantity,
                    AddedAt = c.AddedAt,
                    Product = new Product
                    {
                        Id = c.Product.Id,
                        Name = c.Product.Name,
                        Description = c.Product.Description,
                        Price = c.Product.Price,
                        AvailableQuantity = c.Product.AvailableQuantity,
                        Category = c.Product.Category,
                        ImageUrl = c.Product.ImageUrl
                    }
                })
                .Where(c => c.UserId == userId)
                .ToListAsync();
        }

        public async Task<Cart?> GetByIdAsync(string id)
        {
            return await _context.Carts.FindAsync(id);
        }

        public async Task<Cart?> GetByUserIdAndProductIdAsync(string userId, string productId)
        {
            return await _context.Carts
                .Select(c => new Cart
                {
                    Id = c.Id,
                    UserId = c.UserId,
                    ProductId = c.ProductId,
                    Quantity = c.Quantity,
                    AddedAt = c.AddedAt,
                })
                .Where(c => c.UserId == userId)
                .Where(c => c.ProductId == productId)
                .FirstOrDefaultAsync();
        }

        public async Task AddAsync(Cart cart)
        {
            var cartToAdd = new Cart
            {
                Id = cart.Id,
                UserId = cart.UserId,
                ProductId = cart.ProductId,
                Quantity = cart.Quantity,
                AddedAt = cart.AddedAt
            };
            await _context.Carts.AddAsync(cartToAdd);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Cart cart)
        {
            _context.Carts.Where(c => c.Id == cart.Id)
            .ExecuteUpdate(c => c
            .SetProperty(c => c.Quantity, cart.Quantity)
            );
        }

        public async Task DeleteAsync(string id)
        {
            var cart = await _context.Carts.FindAsync(id);
            if (cart != null)
            {
                _context.Carts.Remove(cart);
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteAllByUserIdAsync(string userId)
        {
            var cartItems = await _context.Carts
                .Where(c => c.UserId == userId)
                .ToListAsync();

            _context.Carts.RemoveRange(cartItems);
            await _context.SaveChangesAsync();
        }
    }
} 