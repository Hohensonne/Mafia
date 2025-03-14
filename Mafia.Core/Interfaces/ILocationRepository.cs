using Mafia.Core.Models;

namespace Mafia.Core.Interfaces
{
    public interface ILocationRepository
    {
        Task<IEnumerable<Location>> GetAllAsync();
        Task<Location?> GetByIdAsync(string id);
        Task<Location?> GetByIdWithGamesAsync(string id);
        Task<string> CreateAsync(Location location);
        Task UpdateAsync(Location location);
        Task DeleteAsync(string id);
    }
} 