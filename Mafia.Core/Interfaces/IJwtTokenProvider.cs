using Mafia.Core.Models;

namespace Mafia.Core.Interfaces
{
    public interface IJwtTokenProvider
    {
        string GenerateJwtToken(User user, IList<string> roles);
        string GenerateRefreshToken();
        Task SaveRefreshTokenAsync(User user);
        Task<User?> GetRefreshTokenAsync(string refreshToken);
        Task RemoveRefreshTokenAsync(User user);
    }
}