using Mafia.Core.Models;

namespace Mafia.Core.Interfaces
{
    public interface IGameRepository
    {
        Task<IEnumerable<Game>> GetAllAsync();
        Task<IEnumerable<Game>> GetAllByLocationIdAsync(string locationId);
        Task<IEnumerable<Game>> GetUpcomingGamesAsync();
        Task<Game?> GetByIdAsync(string id);
        Task<Game?> GetByIdWithRegistrationsAsync(string id);
        Task<Game?> GetByIdWithPhotosAsync(string id);
        Task<string> CreateAsync(Game game);
        Task UpdateAsync(Game game);
        Task DeleteAsync(string id);
        Task<bool> IncrementCurrentPlayersAsync(string id);
        Task<bool> DecrementCurrentPlayersAsync(string id);
    }
} 