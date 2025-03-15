using Mafia.Core.Models;
using Microsoft.AspNetCore.Http;

namespace Mafia.Core.Interfaces
{
    public interface IUsersService
    {
        Task<User> GetByEmail(string email);
        Task DeleteByEmail(string email);
        Task<(string jwtToken, string refreshToken)> Login(string email, string password);
        Task Register(string userName, string email, string password, IFormFile profileImage);
        Task Update(string id, string userName, string email, string password);
        Task UpdateProfileImage(string email, IFormFile profileImage);
        Task<(string jwtToken, string refreshToken)> RefreshTokenAsync(string refreshToken);
        Task<IEnumerable<User>> GetAll();
    }
}