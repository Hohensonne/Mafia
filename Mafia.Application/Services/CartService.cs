using Mafia.Core.Interfaces;
using Mafia.Core.Models;

namespace Mafia.Application.Services
{
    public class CartService : ICartService
    {
        private readonly ICartRepository _cartRepository;
        private readonly IProductRepository _productRepository;

        public CartService(ICartRepository cartRepository, IProductRepository productRepository)
        {
            _cartRepository = cartRepository;
            _productRepository = productRepository;
        }

        public async Task<IEnumerable<Cart>> GetUserCartAsync(string userId)
        {
            return await _cartRepository.GetAllByUserIdAsync(userId);
        }

        public async Task<Cart> UpdateCartItemAsync(string userId, string productId, int quantity)
        {
            if (quantity < 0)
            {
                throw new InvalidOperationException("Quantity cant be negative");
            }
            var cartItem = await _cartRepository.GetByUserIdAndProductIdAsync(userId, productId);
            if (cartItem == null && quantity == 0)
            {
                throw new InvalidOperationException("Cart item not found and cant be deleted");
            }

            var product = await _productRepository.GetByIdAsync(productId);
            if (product == null)
            {
                throw new InvalidOperationException("Product not found");
            }
            if (quantity > product.AvailableQuantity)
            {
                throw new InvalidOperationException("Not enough items in stock");
            }
            if (quantity == 0)
            {
                await _cartRepository.DeleteAsync(cartItem.Id);
                return null;
            }
            if (cartItem == null)
            {
                cartItem = new Cart
                {
                    Id = Guid.NewGuid().ToString(),
                    UserId = userId,
                    ProductId = productId,
                    Quantity = quantity,
                    AddedAt = DateTime.UtcNow
                };
                await _cartRepository.AddAsync(cartItem);
                return cartItem;
            }
            else
            {
                cartItem.Quantity = quantity;
                await _cartRepository.UpdateAsync(cartItem);
                return cartItem;
            }
        }

        

        public async Task<bool> RemoveFromCartAsync(string cartId)
        {
            var cartItem = await _cartRepository.GetByIdAsync(cartId);
            if (cartItem == null)
            {
                return false;
            }

            await _cartRepository.DeleteAsync(cartId);
            return true;
        }

        public async Task ClearCartAsync(string userId)
        {
            await _cartRepository.DeleteAllByUserIdAsync(userId);
        }
    }
} 