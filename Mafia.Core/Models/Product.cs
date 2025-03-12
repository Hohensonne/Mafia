using System;

namespace Mafia.Core.Models;

public class Product
{
    public required Guid Id { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }
    public required double Price { get; set; }
    public required int AvailableQuantity { get; set; }
    public string? Category { get; set; }
    public required string ImageUrl { get; set; }
}
