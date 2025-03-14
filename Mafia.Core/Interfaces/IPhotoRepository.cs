using Mafia.Core.Models;

namespace Mafia.Core.Interfaces
{
    public interface IPhotoRepository
    {
        Task<IEnumerable<Photo>> GetAllAsync();
        Task<IEnumerable<Photo>> GetAllByGameIdAsync(string gameId);
        Task<IEnumerable<Photo>> GetAllByUserIdAsync(string userId);
        Task<Photo?> GetByIdAsync(string id);
        Task<string> CreateAsync(Photo photo);
        Task UpdateAsync(Photo photo);
        Task DeleteAsync(string id);
    }
} 