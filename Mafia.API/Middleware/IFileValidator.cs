using Microsoft.AspNetCore.Http;

namespace Mafia.API.Middleware
{
    public interface IFileValidator
    {
        bool IsValid(IFormFile file);
    }
} 