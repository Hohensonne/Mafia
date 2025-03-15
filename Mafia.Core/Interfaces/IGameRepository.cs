using Mafia.Core.Models;

namespace Mafia.Core.Interfaces
{
    public interface IGameRepository
    {
        Task<IEnumerable<Game>> GetAllAsync();
        Task<IEnumerable<Game>> GetUpcomingGamesAsync();
        Task<Game?> GetByIdAsync(string id);
        Task<IEnumerable<Game>> GetRegisteredGamesAsync(string userId);
        Task<Game?> GetByIdWithPhotosAsync(string id);
        Task<string> CreateAsync(Game game);
        Task<string> UpdateAsync(Game game);
        Task<string> DeleteAsync(string id);
        Task IncrementPlayersAsync(string id);
        Task DecrementPlayersAsync(string id);
    }
} 