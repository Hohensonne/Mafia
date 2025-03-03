using Microsoft.AspNetCore.Identity;

namespace Mafia.Core.Models
{
    public class User : IdentityUser
    {
        public string? ProfileImageUrl { get; set; }
    }
}
