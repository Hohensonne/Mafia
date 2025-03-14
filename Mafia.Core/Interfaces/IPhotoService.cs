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
        Task<string> UploadPhotoAsync(string userId, string gameId, IFormFile file);
        Task DeletePhotoAsync(string id);
    }
} 