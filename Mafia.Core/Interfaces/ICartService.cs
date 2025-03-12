using Mafia.Core.Models;

namespace Mafia.Core.Interfaces
{
    public interface ICartService
    {
        Task<IEnumerable<Cart>> GetUserCartAsync(string userId);
        Task<bool> AddToCartAsync(string userId, Guid productId, int quantity);
        Task<bool> UpdateCartItemAsync(Guid cartId, int quantity);
        Task<bool> RemoveFromCartAsync(Guid cartId);
        Task ClearCartAsync(string userId);
    }
} 