using Mafia.Core.Models;

namespace Mafia.Core.Interfaces
{
    public interface IOrderRepository
    {
        Task<IEnumerable<Order>> GetAllAsync();
        Task<IEnumerable<Order>> GetAllByUserIdAsync(string userId);
        Task<Order?> GetByIdAsync(Guid id);
        Task<Order?> GetByIdWithDetailsAsync(Guid id);
        Task<Guid> CreateAsync(Order order);
        Task UpdateAsync(Order order);
        Task DeleteAsync(Guid id);
    }
} 