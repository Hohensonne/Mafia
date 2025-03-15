using Mafia.Core.Models;

namespace Mafia.Core.Interfaces;

public interface IGameRegistrationService
{
    Task<IEnumerable<GameRegistration>> GetAllAsync();
    Task<IEnumerable<GameRegistration>> GetAllByGameIdAsync(string gameId);
    Task<IEnumerable<GameRegistration>> GetAllByUserIdAsync(string userId);
    Task<GameRegistration?> GetByIdAsync(string gameId);
    Task<GameRegistration?> GetByGameIdAndUserIdAsync(string gameId, string userId);
}