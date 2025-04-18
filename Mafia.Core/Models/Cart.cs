using System;

namespace Mafia.Core.Models;

public class Cart
{
    public string Id { get; set; }
    public string UserId { get; set; }
    public User User { get; set; }
    public string ProductId { get; set; }
    public Product Product { get; set; }
    public int Quantity { get; set; }
    public DateTime AddedAt { get; set; }
}
