using Mafia.Core.Interfaces;
using Mafia.Core.Models;

namespace Mafia.Application.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IOrderDetailRepository _orderDetailRepository;
        private readonly ICartRepository _cartRepository;
        private readonly IProductRepository _productRepository;

        public OrderService(
            IOrderRepository orderRepository,
            IOrderDetailRepository orderDetailRepository,
            ICartRepository cartRepository,
            IProductRepository productRepository)
        {
            _orderRepository = orderRepository;
            _orderDetailRepository = orderDetailRepository;
            _cartRepository = cartRepository;
            _productRepository = productRepository;
        }

        public async Task<IEnumerable<Order>> GetAllOrdersAsync()
        {
            return await _orderRepository.GetAllAsync();
        }

        public async Task<IEnumerable<Order>> GetUserOrdersAsync(string userId)
        {
            return await _orderRepository.GetAllByUserIdAsync(userId);
        }

        public async Task<Order?> GetOrderByIdAsync(Guid id)
        {
            return await _orderRepository.GetByIdAsync(id);
        }

        public async Task<Order?> GetOrderWithDetailsAsync(Guid id)
        {
            return await _orderRepository.GetByIdWithDetailsAsync(id);
        }

        public async Task<Guid> CreateOrderFromCartAsync(string userId, PaymentMethodType paymentMethod)
        {
            // Получаем корзину пользователя
            var cartItems = await _cartRepository.GetAllByUserIdAsync(userId);
            if (!cartItems.Any())
            {
                throw new InvalidOperationException("Корзина пуста");
            }

            // Проверяем наличие товаров
            foreach (var item in cartItems)
            {
                var product = await _productRepository.GetByIdAsync(item.ProductId);
                if (product == null || product.AvailableQuantity < item.Quantity)
                {
                    throw new InvalidOperationException($"Товар {product?.Name ?? "неизвестный"} недоступен в запрошенном количестве");
                }
            }

            // Создаем заказ
            var order = new Order
            {
                UserId = userId,
                OrderDate = DateTime.UtcNow,
                TotalAmount = 0, // Рассчитаем позже
                Status = "Создан"
            };

            var orderId = await _orderRepository.CreateAsync(order);

            // Создаем детали заказа
            double totalAmount = 0;
            foreach (var item in cartItems)
            {
                var product = await _productRepository.GetByIdAsync(item.ProductId);
                
                var orderDetail = new OrderDetail
                {
                    OrderId = orderId,
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    Price = product.Price
                };

                await _orderDetailRepository.CreateAsync(orderDetail);

                // Обновляем количество товара
                await _productRepository.UpdateStockAsync(item.ProductId, -item.Quantity);

                // Рассчитываем общую сумму
                totalAmount += product.Price * item.Quantity;
            }

            // Обновляем общую сумму заказа
            order.Id = orderId;
            order.TotalAmount = totalAmount;
            await _orderRepository.UpdateAsync(order);

            // Очищаем корзину
            await _cartRepository.ClearCartAsync(userId);

            return orderId;
        }

        public async Task UpdateOrderStatusAsync(Guid orderId, string status)
        {
            var order = await _orderRepository.GetByIdAsync(orderId);
            if (order == null)
            {
                throw new InvalidOperationException("Заказ не найден");
            }

            order.Status = status;
            await _orderRepository.UpdateAsync(order);
        }

        public async Task CancelOrderAsync(Guid orderId)
        {
            var order = await _orderRepository.GetByIdWithDetailsAsync(orderId);
            if (order == null)
            {
                throw new InvalidOperationException("Заказ не найден");
            }

            // Проверяем, можно ли отменить заказ
            if (order.Status == "Доставлен" || order.Status == "Отменен")
            {
                throw new InvalidOperationException("Невозможно отменить заказ в текущем статусе");
            }

            // Возвращаем товары на склад
            foreach (var detail in order.OrderDetails)
            {
                await _productRepository.UpdateStockAsync(detail.ProductId, detail.Quantity);
            }

            // Обновляем статус заказа
            order.Status = "Отменен";
            await _orderRepository.UpdateAsync(order);
        }
    }
} 