using Mafia.Core.Models;

namespace Mafia.Core.Interfaces
{
    public interface IGameRepository
    {
        Task<IEnumerable<Game>> GetAllAsync();
        Task<IEnumerable<Game>> GetAllByLocationIdAsync(Guid locationId);
        Task<IEnumerable<Game>> GetUpcomingGamesAsync();
        Task<Game?> GetByIdAsync(Guid id);
        Task<Game?> GetByIdWithRegistrationsAsync(Guid id);
        Task<Game?> GetByIdWithPhotosAsync(Guid id);
        Task<Guid> CreateAsync(Game game);
        Task UpdateAsync(Game game);
        Task DeleteAsync(Guid id);
        Task<bool> IncrementCurrentPlayersAsync(Guid id);
        Task<bool> DecrementCurrentPlayersAsync(Guid id);
    }
} 