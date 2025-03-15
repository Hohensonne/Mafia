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
        Task<Order?> GetOrderByIdAsync(string id);
        Task<Order?> GetOrderWithDetailsAsync(string id);
        Task<string> CreateOrderFromCartAsync(string userId);
        Task UpdateOrderStatusAsync(string orderId, string status);
        Task CancelOrderAsync(string orderId);
        Task CancelOrderAsync(string orderId, string userId);
    }
} 