namespace Mafia.API.Contracts;

public record class UpdateProductRequest
{
    public required string Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public double? Price { get; set; }
    public int? AvailableQuantity { get; set; }
    public string? Category { get; set; }
    public IFormFile? Image { get; set; }
}
