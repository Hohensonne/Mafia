using System;

namespace Mafia.Core.Models;

public class Product
{
    public required string Id { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }
    public required double Price { get; set; }
    public required int AvailableQuantity { get; set; }
    public string? Category { get; set; }
    public required string ImageUrl { get; set; }
    public ICollection<Cart> Carts { get; set; }
    public ICollection<OrderDetail> OrderDetails { get; set; }
}
