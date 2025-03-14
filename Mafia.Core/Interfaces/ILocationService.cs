using Mafia.Core.Models;

namespace Mafia.Core.Interfaces
{
    public interface ILocationService
    {
        Task<IEnumerable<Location>> GetAllLocationsAsync();
        Task<Location?> GetLocationByIdAsync(string id);
        Task<Location?> GetLocationWithGamesAsync(string id);
        Task<string> CreateLocationAsync(Location location);
        Task UpdateLocationAsync(Location location);
        Task DeleteLocationAsync(string id);
    }
} 