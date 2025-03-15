using Mafia.Core.Models;

namespace Mafia.Core.Interfaces
{
    public interface IOrderDetailRepository
    {
        Task<IEnumerable<OrderDetail>> GetAllByOrderIdAsync(string orderId);
        Task<OrderDetail?> GetByIdAsync(string id);
        Task<string> CreateAsync(OrderDetail orderDetail);
        Task UpdateAsync(OrderDetail orderDetail);
        Task DeleteAsync(string id);
        Task DeleteByOrderIdAsync(string orderId);
    }
} 