using Mafia.Core.Models;
using Microsoft.AspNetCore.Http;

namespace Mafia.Core.Interfaces
{
    public interface IGameService
    {
        Task<IEnumerable<Game>> GetAllGamesAsync();
        Task<IEnumerable<Game>> GetUpcomingGamesAsync();
        Task<Game?> GetGameByIdAsync(string id);
        Task<IEnumerable<Game>> GetRegisteredGamesAsync(string userId);
        Task<Game?> GetGameWithPhotosAsync(string id);
        Task<string> CreateGameAsync(string name, DateTime startTime, DateTime endOfRegistration, int maxPlayers);
        Task<string> UpdateGameAsync(string id, string name, DateTime startTime, DateTime endOfRegistration, int maxPlayers);
        Task<string> DeleteGameAsync(string id);
        Task<string> RegisterUserForGameAsync(string gameId, string userId);
        Task<string> CancelRegistrationAsync(string gameId, string userId);
        Task<string> ApproveRegistrationAsync(string registrationId);
    }
} 