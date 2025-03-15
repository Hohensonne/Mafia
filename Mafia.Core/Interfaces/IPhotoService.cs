using Mafia.Core.Models;
using Microsoft.AspNetCore.Http;

namespace Mafia.Core.Interfaces
{
    public interface IPhotoService
    {
        Task<IEnumerable<Photo>> GetAllPhotosAsync();
        Task<IEnumerable<Photo>> GetPhotosByGameAsync(string gameId);
        Task<IEnumerable<Photo>> GetPhotosByUserAsync(string userId);
        Task<Photo?> GetPhotoByIdAsync(string id);
        Task<string> AddPhotoToGameAsync(string gameId, string userId, IFormFile file);
        Task<string> DeletePhotoAsync(string photoId, string userId);
    }
} 