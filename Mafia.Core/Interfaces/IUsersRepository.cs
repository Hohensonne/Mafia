using Mafia.Core.Models;
using Microsoft.AspNetCore.Identity;

namespace Mafia.Core.Interfaces
{
    public interface IUsersRepository
    {
        Task<IdentityResult> Create(User user, string password);
        Task<IdentityResult> DeleteByEmail(string email);
        Task<IList<User>> GetAll();
        Task<User?> GetByEmail(string email);
        Task<IdentityResult> Update(string id, string userName, string email, string password);
        Task<IdentityResult> UpdateProfileImage(string id, string profileImageUrl);
    }
}