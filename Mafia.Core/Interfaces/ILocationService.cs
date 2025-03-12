using Mafia.Core.Models;

namespace Mafia.Core.Interfaces
{
    public interface ILocationService
    {
        Task<IEnumerable<Location>> GetAllLocationsAsync();
        Task<Location?> GetLocationByIdAsync(Guid id);
        Task<Location?> GetLocationWithGamesAsync(Guid id);
        Task<Guid> CreateLocationAsync(Location location);
        Task UpdateLocationAsync(Location location);
        Task DeleteLocationAsync(Guid id);
    }
} 