using Microsoft.AspNetCore.Http;

namespace Mafia.Core.Interfaces;

public interface IFileRepository
{
    Task<string> SaveProfileImageAsync(string userId, IFormFile file);
    Task DeleteProfileImageAsync(string imageUrl);
}
