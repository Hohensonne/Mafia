using System;

namespace Mafia.Core.Models;

public class OrderDetail
{
    public string Id { get; set; }
    public string OrderId { get; set; }
    public Order Order { get; set; }
    public string ProductId { get; set; }
    public Product Product { get; set; }
    public int Quantity { get; set; }
    public double Price { get; set; }
}
