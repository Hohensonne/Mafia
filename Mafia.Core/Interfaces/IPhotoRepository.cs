using Mafia.Core.Models;

namespace Mafia.Core.Interfaces
{
    public interface IPhotoRepository
    {
        Task<IEnumerable<Photo>> GetAllAsync();
        Task<IEnumerable<Photo>> GetAllByGameIdAsync(Guid gameId);
        Task<IEnumerable<Photo>> GetAllByUserIdAsync(string userId);
        Task<Photo?> GetByIdAsync(Guid id);
        Task<Guid> CreateAsync(Photo photo);
        Task UpdateAsync(Photo photo);
        Task DeleteAsync(Guid id);
    }
} 