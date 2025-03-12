using Microsoft.AspNetCore.Identity;

namespace Mafia.Core.Models
{
    public class User : IdentityUser
    {
        public string? ProfileImageUrl { get; set; }
        public DateTime? RegistrationDate { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiryTime { get; set; } 
    }
}
