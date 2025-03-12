using Mafia.Core.Models;

namespace Mafia.Core.Interfaces
{
    public interface ICartRepository
    {
        Task<IEnumerable<Cart>> GetAllByUserIdAsync(string userId);
        Task<Cart?> GetByIdAsync(Guid id);
        Task<Cart?> GetByUserIdAndProductIdAsync(string userId, Guid productId);
        Task AddAsync(Cart cart);
        Task UpdateAsync(Cart cart);
        Task DeleteAsync(Guid id);
        Task ClearCartAsync(string userId);
    }
} 