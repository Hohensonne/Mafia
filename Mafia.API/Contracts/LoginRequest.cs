using System.ComponentModel.DataAnnotations;

namespace Mafia.API.Contracts
{
    public record LoginRequest([Required] string Email, [Required] string Password);
}
