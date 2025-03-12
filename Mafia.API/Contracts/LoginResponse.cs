namespace Mafia.API.Contracts
{
    public record LoginResponse(string JwtToken, string RefreshToken);
}
