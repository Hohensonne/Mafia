using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Mafia.API.Contracts
{
    public record RegistrationRequest(
        [Required] string FirstName, 
        [Required] string LastName, 
        [Required] string Email, 
        [Required] string Password, 
        [Required] string PhoneNumber,
        IFormFile? ProfileImage);
}
