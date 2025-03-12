using Mafia.Core.Interfaces;
using Mafia.Core.Models;

namespace Mafia.Application.Services
{
    public class GameService : IGameService
    {
        private readonly IGameRepository _gameRepository;
        private readonly IGameRegistrationRepository _gameRegistrationRepository;
        private readonly IPhotoRepository _photoRepository;

        public GameService(
            IGameRepository gameRepository,
            IGameRegistrationRepository gameRegistrationRepository,
            IPhotoRepository photoRepository)
        {
            _gameRepository = gameRepository;
            _gameRegistrationRepository = gameRegistrationRepository;
            _photoRepository = photoRepository;
        }

        public async Task<IEnumerable<Game>> GetAllGamesAsync()
        {
            return await _gameRepository.GetAllAsync();
        }

        public async Task<IEnumerable<Game>> GetGamesByLocationAsync(Guid locationId)
        {
            return await _gameRepository.GetAllByLocationIdAsync(locationId);
        }

        public async Task<IEnumerable<Game>> GetUpcomingGamesAsync()
        {
            return await _gameRepository.GetUpcomingGamesAsync();
        }

        public async Task<Game?> GetGameByIdAsync(Guid id)
        {
            return await _gameRepository.GetByIdAsync(id);
        }

        public async Task<Game?> GetGameWithRegistrationsAsync(Guid id)
        {
            return await _gameRepository.GetByIdWithRegistrationsAsync(id);
        }

        public async Task<Game?> GetGameWithPhotosAsync(Guid id)
        {
            return await _gameRepository.GetByIdWithPhotosAsync(id);
        }

        public async Task<Guid> CreateGameAsync(Game game)
        {
            game.CreatedAt = DateTime.UtcNow;
            game.CurrentPlayers = 0;
            return await _gameRepository.CreateAsync(game);
        }

        public async Task UpdateGameAsync(Game game)
        {
            await _gameRepository.UpdateAsync(game);
        }

        public async Task DeleteGameAsync(Guid id)
        {
            await _gameRepository.DeleteAsync(id);
        }

        public async Task<bool> RegisterUserForGameAsync(Guid gameId, string userId)
        {
            // Проверяем, существует ли игра
            var game = await _gameRepository.GetByIdAsync(gameId);
            if (game == null)
            {
                return false;
            }

            // Проверяем, не закончилась ли регистрация
            if (game.EndOfRegistration < DateTime.UtcNow)
            {
                return false;
            }

            // Проверяем, не заполнена ли игра
            if (game.CurrentPlayers >= game.MaxPlayers)
            {
                return false;
            }

            // Проверяем, не зарегистрирован ли уже пользователь
            var existingRegistration = await _gameRegistrationRepository.GetByGameIdAndUserIdAsync(gameId, userId);
            if (existingRegistration != null)
            {
                return false;
            }

            // Создаем регистрацию
            var registration = new GameRegistration
            {
                GameId = gameId,
                UserId = userId,
                CreatedAt = DateTime.UtcNow,
                IsApproved = false
            };

            await _gameRegistrationRepository.CreateAsync(registration);

            // Увеличиваем счетчик игроков
            await _gameRepository.IncrementCurrentPlayersAsync(gameId);

            return true;
        }

        public async Task<bool> CancelRegistrationAsync(Guid gameId, string userId)
        {
            var registration = await _gameRegistrationRepository.GetByGameIdAndUserIdAsync(gameId, userId);
            if (registration == null)
            {
                return false;
            }

            await _gameRegistrationRepository.DeleteAsync(registration.Id);

            // Уменьшаем счетчик игроков
            await _gameRepository.DecrementCurrentPlayersAsync(gameId);

            return true;
        }

        public async Task<bool> ApproveRegistrationAsync(Guid registrationId)
        {
            var registration = await _gameRegistrationRepository.GetByIdAsync(registrationId);
            if (registration == null)
            {
                return false;
            }

            await _gameRegistrationRepository.ApproveAsync(registrationId);
            return true;
        }

        public async Task<bool> AddPhotoToGameAsync(Photo photo)
        {
            var game = await _gameRepository.GetByIdAsync(photo.GameId);
            if (game == null)
            {
                return false;
            }

            photo.UploadedAt = DateTime.UtcNow;
            await _photoRepository.CreateAsync(photo);
            return true;
        }
    }
} 