using Microsoft.AspNetCore.Identity;

namespace Mafia.Core.Models
{
    public class User : IdentityUser
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? ProfileImageUrl { get; set; }
        public DateTime? RegistrationDate { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiryTime { get; set; } 
        public ICollection<GameRegistration> GameRegistrations { get; set; }
        public ICollection<Photo> Photos { get; set; }
        public ICollection<Cart> Carts { get; set; }
    }
}