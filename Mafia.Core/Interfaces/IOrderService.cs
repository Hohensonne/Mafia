using Mafia.Core.Models;

namespace Mafia.Core.Interfaces
{
    public enum PaymentMethodType
    {
        Cash,
        CreditCard,
        DebitCard,
        ElectronicWallet,
        BankTransfer
    }

    public interface IOrderService
    {
        Task<IEnumerable<Order>> GetAllOrdersAsync();
        Task<IEnumerable<Order>> GetUserOrdersAsync(string userId);
        Task<Order?> GetOrderByIdAsync(Guid id);
        Task<Order?> GetOrderWithDetailsAsync(Guid id);
        Task<Guid> CreateOrderFromCartAsync(string userId, PaymentMethodType paymentMethod);
        Task UpdateOrderStatusAsync(Guid orderId, string status);
        Task CancelOrderAsync(Guid orderId);
    }
} 