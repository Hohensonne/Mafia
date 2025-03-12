using Mafia.Core.Models;

namespace Mafia.Core.Interfaces
{
    public interface ILocationRepository
    {
        Task<IEnumerable<Location>> GetAllAsync();
        Task<Location?> GetByIdAsync(Guid id);
        Task<Location?> GetByIdWithGamesAsync(Guid id);
        Task<Guid> CreateAsync(Location location);
        Task UpdateAsync(Location location);
        Task DeleteAsync(Guid id);
    }
} 