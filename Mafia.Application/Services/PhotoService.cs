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

        public async Task<string> UploadPhotoAsync(string userId, string gameId, IFormFile file)
        {
            // Проверяем, существует ли игра
            var game = await _gameRepository.GetByIdAsync(gameId);
            if (game == null)
            {
                throw new InvalidOperationException("Игра не найдена");
            }

            // Сохраняем файл
            string url = await _fileRepository.SaveProfileImageAsync(userId, file);

            // Создаем запись о фото
            var photo = new Photo
            {
                ImageUrl = url,
                GameId = gameId,
                UserId = userId,
                UploadedAt = DateTime.UtcNow
            };

            return await _photoRepository.CreateAsync(photo);
        }

        public async Task DeletePhotoAsync(string id)
        {
            var photo = await _photoRepository.GetByIdAsync(id);
            if (photo == null)
            {
                throw new InvalidOperationException("Фото не найдено");
            }

            // Удаляем файл
            await _fileRepository.DeleteProfileImageAsync(photo.ImageUrl);

            // Удаляем запись
            await _photoRepository.DeleteAsync(id);
        }
    }
} 