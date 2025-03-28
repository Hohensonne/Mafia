using Mafia.Core.Interfaces;
using Mafia.Core.Models;

namespace Mafia.Application.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ICartRepository _cartRepository;
        private readonly IProductRepository _productRepository;
        private readonly IOrderDetailRepository _orderDetailRepository;
        private readonly IExcelService _excelService;

        public OrderService(
            IOrderRepository orderRepository,
            ICartRepository cartRepository,
            IProductRepository productRepository,
            IOrderDetailRepository orderDetailRepository,
            IExcelService excelService)
        {
            _orderRepository = orderRepository;
            _cartRepository = cartRepository;
            _productRepository = productRepository;
            _orderDetailRepository = orderDetailRepository;
            _excelService = excelService;
        }

        public async Task<IEnumerable<Order>> GetAllOrdersAsync()
        {
            return await _orderRepository.GetAllAsync();
        }

        public async Task<IEnumerable<Order>> GetUserOrdersAsync(string userId)
        {
            return await _orderRepository.GetAllByUserIdAsync(userId);
        }

        public async Task<Order?> GetOrderByIdAsync(string id)
        {
            return await _orderRepository.GetByIdAsync(id);
        }

        public async Task<Order?> GetOrderWithDetailsAsync(string id)
        {
            return await _orderRepository.GetByIdWithDetailsAsync(id);
        }

        public async Task<string> CreateOrderFromCartAsync(string userId, string address, PaymenthMethod paymentMethod)
        {
            var cartItems = await _cartRepository.GetAllByUserIdAsync(userId);
            if (!cartItems.Any())
            {
                throw new InvalidOperationException("Cart is empty");
            }

            foreach (var item in cartItems)
            {
                var product = await _productRepository.GetByIdAsync(item.ProductId);
                if (product == null || product.AvailableQuantity < item.Quantity)
                {
                    throw new InvalidOperationException($"This item is out of stock {product?.Name ?? "unknown"} in the requested quantity");
                }
            }
            
            var order = new Order
            {
                Id = Guid.NewGuid().ToString(),
                UserId = userId,
                OrderDate = DateTime.UtcNow,
                Status = OrderStatusEnum.Created,
                Address = address,
                PaymentMethod = paymentMethod
            };

            await _orderRepository.CreateAsync(order);

            double totalAmount = 0;
            foreach (var item in cartItems)
            {
                var product = await _productRepository.GetByIdAsync(item.ProductId);
                
                var orderDetail = new OrderDetail
                {
                    Id = Guid.NewGuid().ToString(),
                    OrderId = order.Id,
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    Price = product.Price
                };

                await _orderDetailRepository.CreateAsync(orderDetail);

                await _productRepository.UpdateStockAsync(item.ProductId, product.AvailableQuantity - item.Quantity);

                totalAmount += product.Price * item.Quantity;
            }

            order.TotalAmount = totalAmount;
            await _orderRepository.UpdateAsync(order);

            await _cartRepository.DeleteAllByUserIdAsync(userId);

            return order.Id;
        }

        public async Task UpdateOrderStatusAsync(string orderId, string status)
        {
            var order = await _orderRepository.GetByIdWithDetailsAsync(orderId);
            if (order == null)
            {
                throw new InvalidOperationException("order not found");
            }

            order.Status = (OrderStatusEnum)Enum.Parse(typeof(OrderStatusEnum), status);
            if (order.Status == OrderStatusEnum.Cancelled)
            {
                await CancelOrderAsync(orderId);
                return;
            }

            await _orderRepository.UpdateAsync(order);

            // Если заказ доставлен, записываем его в Excel
            if (order.Status == OrderStatusEnum.Delivered)
            {
                try
                {
                    await _excelService.SaveDeliveredOrderToExcelAsync(order);
                }
                catch (Exception ex)
                {
                    // Логируем ошибку, но не прерываем выполнение основной операции
                    // TODO: добавить логирование
                    Console.WriteLine($"Error saving order to Excel: {ex.Message}");
                }
            }
        }

        public async Task CancelOrderAsync(string orderId, string userId)
        {
            var order = await _orderRepository.GetByIdWithDetailsAsync(orderId);
            if (order == null)
            {
                throw new InvalidOperationException("order not found");
            }

            if (order.UserId != userId)
            {
                throw new InvalidOperationException("You cannot cancel this order");
            }
            await CancelOrderAsync(orderId);
        }
        

        public async Task CancelOrderAsync(string orderId)
        {
            var order = await _orderRepository.GetByIdWithDetailsAsync(orderId);
            if (order == null)
            {
                throw new InvalidOperationException("order not found");
            }

            if (order.Status == OrderStatusEnum.Delivered || order.Status == OrderStatusEnum.Cancelled)
            {
                throw new InvalidOperationException("Cannot cancel order in current status");
            }

            foreach (var detail in order.OrderDetails)
            {
                await _productRepository.UpdateStockAsync(detail.ProductId, detail.Quantity);
            }

            order.Status = OrderStatusEnum.Cancelled;
            await _orderRepository.UpdateAsync(order);
        }
    }
} 