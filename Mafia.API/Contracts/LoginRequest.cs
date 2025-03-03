using System.ComponentModel.DataAnnotations;

namespace Mafia.API.Contracts
{
    public class LoginRequest
    {
        [Required]
        public string Email { get; }
        [Required]
        public string Password { get; }
    }
}
