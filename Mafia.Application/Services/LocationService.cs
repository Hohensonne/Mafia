using Mafia.Core.Interfaces;
using Mafia.Core.Models;

namespace Mafia.Application.Services
{
    public class LocationService : ILocationService
    {
        private readonly ILocationRepository _locationRepository;

        public LocationService(ILocationRepository locationRepository)
        {
            _locationRepository = locationRepository;
        }

        public async Task<IEnumerable<Location>> GetAllLocationsAsync()
        {
            return await _locationRepository.GetAllAsync();
        }

        public async Task<Location?> GetLocationByIdAsync(string id)
        {
            return await _locationRepository.GetByIdAsync(id);
        }

        public async Task<Location?> GetLocationWithGamesAsync(string id)
        {
            return await _locationRepository.GetByIdWithGamesAsync(id);
        }

        public async Task<string> CreateLocationAsync(Location location)
        {
            return await _locationRepository.CreateAsync(location);
        }

        public async Task UpdateLocationAsync(Location location)
        {
            await _locationRepository.UpdateAsync(location);
        }

        public async Task DeleteLocationAsync(string id)
        {
            await _locationRepository.DeleteAsync(id);
        }
    }
} 