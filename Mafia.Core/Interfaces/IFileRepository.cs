using Microsoft.AspNetCore.Http;

namespace Mafia.Core.Interfaces;

public interface IFileRepository
{
    Task<string> SaveProfileImageAsync(string userId, IFormFile file);
    Task DeleteProfileImageAsync(string imageUrl);
    Task<string> SaveGamePhotoAsync(string gameId, IFormFile file);
    Task DeleteGamePhotoAsync(string imageUrl);
    Task<string> SaveProductImageAsync(string productId, IFormFile file);
    Task DeleteProductImageAsync(string imageUrl);
}
