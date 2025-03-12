using Mafia.Core.Models;

namespace Mafia.Core.Interfaces
{
    public interface IGameRegistrationRepository
    {
        Task<IEnumerable<GameRegistration>> GetAllAsync();
        Task<IEnumerable<GameRegistration>> GetAllByGameIdAsync(Guid gameId);
        Task<IEnumerable<GameRegistration>> GetAllByUserIdAsync(string userId);
        Task<GameRegistration?> GetByIdAsync(Guid id);
        Task<GameRegistration?> GetByGameIdAndUserIdAsync(Guid gameId, string userId);
        Task<Guid> CreateAsync(GameRegistration registration);
        Task UpdateAsync(GameRegistration registration);
        Task DeleteAsync(Guid id);
        Task ApproveAsync(Guid id);
    }
} 