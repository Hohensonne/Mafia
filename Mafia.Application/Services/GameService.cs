using Mafia.Core.Interfaces;
using Mafia.Core.Models;
using Microsoft.AspNetCore.Http;

namespace Mafia.Application.Services
{
    public class GameService : IGameService
    {
        private readonly IGameRepository _gameRepository;
        private readonly IGameRegistrationRepository _gameRegistrationRepository;
        private readonly IPhotoRepository _photoRepository;
        private readonly IFileRepository _fileRepository;
        public GameService(
            IGameRepository gameRepository,
            IGameRegistrationRepository gameRegistrationRepository,
            IPhotoRepository photoRepository,
            IFileRepository fileRepository)
        {
            _gameRepository = gameRepository;
            _gameRegistrationRepository = gameRegistrationRepository;
            _photoRepository = photoRepository;
            _fileRepository = fileRepository;
        }

        public async Task<IEnumerable<Game>> GetAllGamesAsync()
        {
            return await _gameRepository.GetAllAsync();
        }


        public async Task<IEnumerable<Game>> GetUpcomingGamesAsync()
        {
            return await _gameRepository.GetUpcomingGamesAsync();
        }

        public async Task<Game?> GetGameByIdAsync(string id)
        {
            return await _gameRepository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<Game>> GetRegisteredGamesAsync(string userId)
        {
            return await _gameRepository.GetRegisteredGamesAsync(userId);
        }

        public async Task<Game?> GetGameWithPhotosAsync(string id)
        {
            return await _gameRepository.GetByIdWithPhotosAsync(id);
        }

        public async Task<string> CreateGameAsync(string name, DateTime startTime, DateTime endOfRegistration, int maxPlayers)
        {
            if (endOfRegistration > startTime)
                throw new ArgumentException("End of registration cannot be after start time");
            var game = new Game
            {
                Id = Guid.NewGuid().ToString(),
                Name = name,
                StartTime = startTime,
                EndOfRegistration = endOfRegistration,
                MaxPlayers = maxPlayers,
                CreatedAt = DateTime.UtcNow,
                CurrentPlayers = 0
            };
            
            return await _gameRepository.CreateAsync(game);
        }

        public async Task<string> UpdateGameAsync(string id, string name, DateTime startTime, DateTime endOfRegistration, int maxPlayers)
        {
            var game = await _gameRepository.GetByIdAsync(id);
            if (game == null)
            {
                throw new InvalidOperationException($"Game not found");
            }
            if (game.Name != name)
            {
                game.Name = name;
            }
            if (game.StartTime != startTime)
            {
                game.StartTime = startTime;
            }
            if (game.EndOfRegistration != endOfRegistration)
            {
                game.EndOfRegistration = endOfRegistration;
            }
            if (game.MaxPlayers != maxPlayers)
            {
                game.MaxPlayers = maxPlayers;
            }
            await _gameRepository.UpdateAsync(game);
            return game.Id;
        }

        public async Task<string> DeleteGameAsync(string id)
        {
            await _gameRepository.DeleteAsync(id);
            return id;
        }


        public async Task<string> RegisterUserForGameAsync(string gameId, string userId)
        {
            var game = await _gameRepository.GetByIdAsync(gameId);
            if (game == null)
            {
                throw new Exception($"Game not found");
            }

            if (game.EndOfRegistration < DateTime.UtcNow)
            {
                throw new Exception($"Game registration has ended");
            }

            if (game.CurrentPlayers >= game.MaxPlayers)
            {
                throw new Exception($"Game is full");
            }

            var existingRegistration = await _gameRegistrationRepository.GetByGameIdAndUserIdAsync(gameId, userId);
            if (existingRegistration != null)
            {
                throw new Exception($"User already registered for this game");
            }

            var registration = new GameRegistration
            {
                Id = Guid.NewGuid().ToString(),
                GameId = gameId,
                UserId = userId,
                CreatedAt = DateTime.UtcNow,
                IsApproved = false
            };

            await _gameRegistrationRepository.CreateAsync(registration);

            await _gameRepository.IncrementPlayersAsync(gameId);
            return gameId;
        }

        public async Task<string> CancelRegistrationAsync(string gameId, string userId)
        {
            var registration = await _gameRegistrationRepository.GetByGameIdAndUserIdAsync(gameId, userId);
            if (registration == null)
            {
                throw new Exception($"Game registration not found");
            }

            await _gameRegistrationRepository.DeleteAsync(registration.Id);

            await _gameRepository.DecrementPlayersAsync(gameId);
            return gameId;
        }

        public async Task<string> ApproveRegistrationAsync(string registrationId)
        {
            var registration = await _gameRegistrationRepository.GetByIdAsync(registrationId);
            if (registration == null)
            {
                throw new InvalidOperationException($"Registration not found");
            }

            await _gameRegistrationRepository.ApproveAsync(registrationId);
            return registrationId;
        }

        
    }
} 