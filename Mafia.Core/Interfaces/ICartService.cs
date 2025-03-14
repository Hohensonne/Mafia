using Mafia.Core.Models;

namespace Mafia.Core.Interfaces
{
    public interface ICartService
    {
        Task<IEnumerable<Cart>> GetUserCartAsync(string userId);
        Task<bool> AddToCartAsync(string userId, string productId, int quantity);
        Task<bool> UpdateCartItemAsync(string cartId, int quantity);
        Task<bool> RemoveFromCartAsync(string cartId);
        Task ClearCartAsync(string userId);
    }
} 