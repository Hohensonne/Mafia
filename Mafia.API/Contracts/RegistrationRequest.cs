using System.ComponentModel.DataAnnotations;

namespace Mafia.API.Contracts
{
    public class RegistrationRequest
    {
        [Required]
        public string UserName { get;}
        [Required]
        public string Email { get;}
        [Required]
        public string Password { get;}
    }
}
