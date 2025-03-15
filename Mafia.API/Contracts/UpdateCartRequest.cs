using System.ComponentModel.DataAnnotations;
namespace Mafia.API.Contracts;

public record class UpdateCartRequest
{
    [Required]
    public string ProductId { get; set; }
    [Required]
    public int Quantity { get; set; }
}
