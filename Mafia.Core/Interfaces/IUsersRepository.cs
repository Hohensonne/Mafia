using Mafia.Core.Models;

namespace Mafia.Core.Interfaces
{
    public interface IUsersRepository
    {
        Task Create(User user, string password);
        Task DeleteByEmail(string email);
        Task<IList<User>> GetAll();
        Task<User?> GetByEmail(string email);
        Task Upadate(string id, string userName, string email, string password);
    }
}