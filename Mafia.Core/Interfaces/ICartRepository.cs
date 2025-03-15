using Mafia.Core.Models;

namespace Mafia.Core.Interfaces
{
    public interface ICartRepository
    {
        Task<IEnumerable<Cart>> GetAllByUserIdAsync(string userId);
        Task<Cart?> GetByIdAsync(string id);
        Task<Cart?> GetByUserIdAndProductIdAsync(string userId, string productId);
        Task AddAsync(Cart cart);
        Task UpdateAsync(Cart cart);
        Task DeleteAsync(string id);
        Task DeleteAllByUserIdAsync(string userId);
    }
} 