namespace Mafia.API.Contracts;

public record class CreateProductRequest
{
    public required string Name { get; set; }
    public string? Description { get; set; }
    public required double Price { get; set; }
    public required int AvailableQuantity { get; set; }
    public string? Category { get; set; }
    public required IFormFile Image { get; set; }
}
