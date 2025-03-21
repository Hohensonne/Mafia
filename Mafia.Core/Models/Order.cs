using System;

namespace Mafia.Core.Models;

public class Order
{
    public string Id { get; set; }
    public string UserId { get; set; }
    public User User { get; set; }
    public DateTime OrderDate { get; set; }
    public double TotalAmount { get; set; }
    public OrderStatusEnum Status { get; set; }
    public ICollection<OrderDetail> OrderDetails { get; set; }
    public string Address { get; set; }
    public PaymenthMethod PaymentMethod { get; set; }
}
