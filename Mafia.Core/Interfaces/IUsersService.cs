namespace Mafia.Core.Interfaces
{
    public interface IUsersService
    {
        Task DeleteByEmail(string email);
        Task<(string JwtToken, string RefreshToken)> Login(string email, string password);
        Task Register(string userName, string email, string password);
        Task Update(string id, string userName, string email, string password);
    }
}