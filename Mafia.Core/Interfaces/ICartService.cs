using Mafia.Core.Models;

namespace Mafia.Core.Interfaces
{
    public interface ICartService
    {
        Task<IEnumerable<Cart>> GetUserCartAsync(string userId);
        Task<Cart> UpdateCartItemAsync(string cartId, string productId, int quantity);
        Task<bool> RemoveFromCartAsync(string cartId);
        Task ClearCartAsync(string userId);
    }
} 