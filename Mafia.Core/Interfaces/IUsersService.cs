using Mafia.Core.Models;
using Microsoft.AspNetCore.Http;

namespace Mafia.Core.Interfaces
{
    public interface IUsersService
    {
        Task<User> GetByEmail(string email);
        Task DeleteByEmail(string email);
        Task<(string jwtToken, string refreshToken, IEnumerable<string> roles)> Login(string email, string password);
        Task Register(string firstName, string lastName, string email, string phoneNumber, string password, IFormFile profileImage);
        Task Update(string id, string firstName, string lastName, string email, string phoneNumber, string password);
        Task UpdateProfileImage(string email, IFormFile profileImage);
        Task<(string jwtToken, string refreshToken, IEnumerable<string> roles)> RefreshTokenAsync(string refreshToken);
        Task<IEnumerable<User>> GetAll();
    }
}