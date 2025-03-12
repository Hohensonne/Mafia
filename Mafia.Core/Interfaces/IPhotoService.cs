using Mafia.Core.Models;
using Microsoft.AspNetCore.Http;

namespace Mafia.Core.Interfaces
{
    public interface IPhotoService
    {
        Task<IEnumerable<Photo>> GetAllPhotosAsync();
        Task<IEnumerable<Photo>> GetPhotosByGameAsync(Guid gameId);
        Task<IEnumerable<Photo>> GetPhotosByUserAsync(string userId);
        Task<Photo?> GetPhotoByIdAsync(Guid id);
        Task<Guid> UploadPhotoAsync(string userId, Guid gameId, IFormFile file);
        Task DeletePhotoAsync(Guid id);
    }
} 