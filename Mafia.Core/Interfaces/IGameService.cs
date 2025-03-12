using Mafia.Core.Models;

namespace Mafia.Core.Interfaces
{
    public interface IGameService
    {
        Task<IEnumerable<Game>> GetAllGamesAsync();
        Task<IEnumerable<Game>> GetGamesByLocationAsync(Guid locationId);
        Task<IEnumerable<Game>> GetUpcomingGamesAsync();
        Task<Game?> GetGameByIdAsync(Guid id);
        Task<Game?> GetGameWithRegistrationsAsync(Guid id);
        Task<Game?> GetGameWithPhotosAsync(Guid id);
        Task<Guid> CreateGameAsync(Game game);
        Task UpdateGameAsync(Game game);
        Task DeleteGameAsync(Guid id);
        Task<bool> RegisterUserForGameAsync(Guid gameId, string userId);
        Task<bool> CancelRegistrationAsync(Guid gameId, string userId);
        Task<bool> ApproveRegistrationAsync(Guid registrationId);
        Task<bool> AddPhotoToGameAsync(Photo photo);
    }
} 