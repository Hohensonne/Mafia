using Mafia.Core.Models;

namespace Mafia.Core.Interfaces
{
    public interface IOrderDetailRepository
    {
        Task<IEnumerable<OrderDetail>> GetAllByOrderIdAsync(Guid orderId);
        Task<OrderDetail?> GetByIdAsync(Guid id);
        Task<Guid> CreateAsync(OrderDetail orderDetail);
        Task UpdateAsync(OrderDetail orderDetail);
        Task DeleteAsync(Guid id);
        Task DeleteByOrderIdAsync(Guid orderId);
    }
} 