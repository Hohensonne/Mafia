using Microsoft.AspNetCore.Identity;

namespace Models
{
    public class User : IdentityUser
    {
        public string? ProfileImageUrl { get; set; } // Ссылка на изображение
    }
}