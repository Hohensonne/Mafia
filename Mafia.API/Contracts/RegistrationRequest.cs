using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Mafia.API.Contracts
{
    public record RegistrationRequest(
        [Required] string UserName, 
        [Required] string Email, 
        [Required] string Password, 
        IFormFile? ProfileImage);
}
