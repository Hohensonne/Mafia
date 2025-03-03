using System.ComponentModel.DataAnnotations;

namespace Mafia.API.Contracts
{
    public class LoginResponse
    {
        required public string JwtToken { get; set; }
        required public string RefreshToken { get; set; }

    }
}
