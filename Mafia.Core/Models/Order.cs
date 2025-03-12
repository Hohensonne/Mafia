using System;

namespace Mafia.Core.Models;

public class Order
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public User User { get; set; }
    public DateTime OrderDate { get; set; }
    public double TotalAmount { get; set; }
    public string Status { get; set; }
    public Enum PaymentMethod { get; set; }    
}
