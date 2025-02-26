using Microsoft.AspNetCore.Identity;

namespace Mafia.Logic.Models
{
    public class User : IdentityUser
    {
        public string? ProfileImageUrl { get; set; }
    }
}
