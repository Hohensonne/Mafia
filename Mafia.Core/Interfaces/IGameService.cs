using Mafia.Core.Models;
using Microsoft.AspNetCore.Http;

namespace Mafia.Core.Interfaces
{
    public interface IGameService
    {
        Task<IEnumerable<Game>> GetAllGamesAsync();
        Task<IEnumerable<Game>> GetGamesByLocationAsync(string locationId);
        Task<IEnumerable<Game>> GetUpcomingGamesAsync();
        Task<Game?> GetGameByIdAsync(string id);
        Task<Game?> GetGameWithRegistrationsAsync(string id);
        Task<Game?> GetGameWithPhotosAsync(string id);
        Task<string> CreateGameAsync(Game game);
        Task UpdateGameAsync(Game game);
        Task DeleteGameAsync(string id);
        Task<bool> RegisterUserForGameAsync(string gameId, string userId);
        Task<bool> CancelRegistrationAsync(string gameId, string userId);
        Task<bool> ApproveRegistrationAsync(string registrationId);
        Task<bool> AddPhotoToGameAsync(string gameId, string userId, IFormFile file);
    }
} 