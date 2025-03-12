using System;

namespace Mafia.Core.Models;

public class Order
{
    public Guid Id { get; set; }
    public string UserId { get; set; }
    public User User { get; set; }
    public DateTime OrderDate { get; set; }
    public double TotalAmount { get; set; }
    public string Status { get; set; } 
    public ICollection<OrderDetail> OrderDetails { get; set; }
}
