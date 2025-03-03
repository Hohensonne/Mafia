using Mafia.Core.Models;

namespace Mafia.Core.Interfaces
{
    public interface IJwtTokenProvider
    {
        string GenerateJwtToken(User user, IList<string> roles);
        string GenerateRefreshToken();
    }
}