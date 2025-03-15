using Mafia.Core.Models;

namespace Mafia.Core.Interfaces
{
    public interface IOrderRepository
    {
        Task<IEnumerable<Order>> GetAllAsync();
        Task<IEnumerable<Order>> GetAllByUserIdAsync(string userId);
        Task<Order?> GetByIdAsync(string id);
        Task<Order?> GetByIdWithDetailsAsync(string id);
        Task<string> CreateAsync(Order order);
        Task UpdateAsync(Order order);
    }
} 