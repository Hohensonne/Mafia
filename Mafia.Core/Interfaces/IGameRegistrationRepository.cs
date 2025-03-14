using Mafia.Core.Models;

namespace Mafia.Core.Interfaces
{
    public interface IGameRegistrationRepository
    {
        Task<IEnumerable<GameRegistration>> GetAllAsync();
        Task<IEnumerable<GameRegistration>> GetAllByGameIdAsync(string gameId);
        Task<IEnumerable<GameRegistration>> GetAllByUserIdAsync(string userId);
        Task<GameRegistration?> GetByIdAsync(string id);
        Task<GameRegistration?> GetByGameIdAndUserIdAsync(string gameId, string userId);
        Task<string> CreateAsync(GameRegistration registration);
        Task UpdateAsync(GameRegistration registration);
        Task DeleteAsync(string id);
        Task ApproveAsync(string id);
    }
} 