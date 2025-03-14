using Mafia.Core.Interfaces;
using Mafia.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace Mafia.Persistence.Repositories
{
    public class OrderDetailRepository : IOrderDetailRepository
    {
        private readonly ApplicationDbContext _context;

        public OrderDetailRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<OrderDetail>> GetAllByOrderIdAsync(string orderId)
        {
            return await _context.OrderDetails
                .Include(od => od.Product)
                .Where(od => od.OrderId == orderId)
                .ToListAsync();
        }

        public async Task<OrderDetail?> GetByIdAsync(string id)
        {
            return await _context.OrderDetails
                .Include(od => od.Product)
                .FirstOrDefaultAsync(od => od.Id == id);
        }

        public async Task<string> CreateAsync(OrderDetail orderDetail)
        {
            await _context.OrderDetails.AddAsync(orderDetail);
            await _context.SaveChangesAsync();
            return orderDetail.Id;
        }

        public async Task UpdateAsync(OrderDetail orderDetail)
        {
            _context.OrderDetails.Update(orderDetail);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(string id)
        {
            var orderDetail = await _context.OrderDetails.FindAsync(id);
            if (orderDetail != null)
            {
                _context.OrderDetails.Remove(orderDetail);
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteByOrderIdAsync(string orderId)
        {
            var orderDetails = await _context.OrderDetails
                .Where(od => od.OrderId == orderId)
                .ToListAsync();

            if (orderDetails.Any())
            {
                _context.OrderDetails.RemoveRange(orderDetails);
                await _context.SaveChangesAsync();
            }
        }
    }
} 