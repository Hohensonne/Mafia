using Mafia.Core.Models;
using Microsoft.AspNetCore.Http;

namespace Mafia.Core.Interfaces
{
    public interface IPhotoRepository
    {
        Task<IEnumerable<Photo>> GetAllAsync();
        Task<IEnumerable<Photo>> GetAllByGameIdAsync(string gameId);
        Task<IEnumerable<Photo>> GetAllByUserIdAsync(string userId);
        Task<Photo?> GetByIdAsync(string id);
        Task<string> CreateAsync(Photo photo);
        Task<string> DeleteAsync(string id);
        
    }
} 