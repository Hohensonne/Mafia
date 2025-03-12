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

        public async Task<bool> AddToCartAsync(string userId, Guid productId, int quantity)
        {
            // Проверяем, существует ли товар
            var product = await _productRepository.GetByIdAsync(productId);
            if (product == null || product.AvailableQuantity < quantity)
            {
                return false;
            }

            // Проверяем, есть ли уже этот товар в корзине
            var existingCartItem = await _cartRepository.GetByUserIdAndProductIdAsync(userId, productId);
            if (existingCartItem != null)
            {
                // Обновляем количество
                existingCartItem.Quantity += quantity;
                await _cartRepository.UpdateAsync(existingCartItem);
            }
            else
            {
                // Создаем новый элемент корзины
                var cartItem = new Cart
                {
                    UserId = userId,
                    ProductId = productId,
                    Quantity = quantity,
                    AddedAt = DateTime.UtcNow
                };
                await _cartRepository.AddAsync(cartItem);
            }

            return true;
        }

        public async Task<bool> UpdateCartItemAsync(Guid cartId, int quantity)
        {
            var cartItem = await _cartRepository.GetByIdAsync(cartId);
            if (cartItem == null)
            {
                return false;
            }

            // Проверяем доступность товара
            var product = await _productRepository.GetByIdAsync(cartItem.ProductId);
            if (product == null || product.AvailableQuantity < quantity)
            {
                return false;
            }

            cartItem.Quantity = quantity;
            await _cartRepository.UpdateAsync(cartItem);
            return true;
        }

        public async Task<bool> RemoveFromCartAsync(Guid cartId)
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
            await _cartRepository.ClearCartAsync(userId);
        }
    }
} 