using Mafia.Core.Interfaces;
using Mafia.Core.Models;
using Microsoft.AspNetCore.Http;

namespace Mafia.Application.Services
{
    public class PhotoService : IPhotoService
    {
        private readonly IPhotoRepository _photoRepository;
        private readonly IGameRepository _gameRepository;
        private readonly IFileRepository _fileRepository;

        public PhotoService(
            IPhotoRepository photoRepository,
            IGameRepository gameRepository,
            IFileRepository fileRepository)
        {
            _photoRepository = photoRepository;
            _gameRepository = gameRepository;
            _fileRepository = fileRepository;
        }

        public async Task<IEnumerable<Photo>> GetAllPhotosAsync()
        {
            return await _photoRepository.GetAllAsync();
        }

        public async Task<IEnumerable<Photo>> GetPhotosByGameAsync(string gameId)
        {
            return await _photoRepository.GetAllByGameIdAsync(gameId);
        }

        public async Task<IEnumerable<Photo>> GetPhotosByUserAsync(string userId)
        {
            return await _photoRepository.GetAllByUserIdAsync(userId);
        }

        public async Task<Photo?> GetPhotoByIdAsync(string id)
        {
            return await _photoRepository.GetByIdAsync(id);
        }

         public async Task<string> AddPhotoToGameAsync(string gameId, string userId, IFormFile file)
        {
            var game = await _gameRepository.GetByIdAsync(gameId);
            if (game == null)
            {
                throw new InvalidOperationException($"Game not found");
            }

            var imageUrl = await _fileRepository.SaveGamePhotoAsync(gameId, file);
    
            var photo = new Photo
            {
                Id = Guid.NewGuid().ToString(),
                GameId = gameId,
                UserId = userId,
                UploadedAt = DateTime.UtcNow,
                ImageUrl = imageUrl
            };
            
            await _photoRepository.CreateAsync(photo);
            return photo.Id;
        }
        public async Task<string> DeletePhotoAsync(string photoId, string userId)
        {
            var photo = await _photoRepository.GetByIdAsync(photoId);
            if (photo == null)
            {
                throw new InvalidOperationException($"Photo not found");
            }
            if (photo.UserId != userId)
            {
                throw new InvalidOperationException($"You are not allowed to delete this photo");
            }
            await _fileRepository.DeleteGamePhotoAsync(photo.ImageUrl);
            await _photoRepository.DeleteAsync(photoId);
            return photoId;
        }
    }
} 