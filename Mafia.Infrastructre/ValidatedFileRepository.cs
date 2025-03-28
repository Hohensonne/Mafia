using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Mafia.Core.Interfaces;

namespace Mafia.Infrastructre
{
    public class ValidatedFileRepository : IFileRepository
    {
        private readonly string _basePath;
        private readonly string _baseUrl;
        private readonly ILogger<ValidatedFileRepository> _logger;
        private readonly HashSet<string> _allowedMimeTypes;

        public ValidatedFileRepository(
            IWebHostEnvironment environment, 
            IConfiguration configuration,
            ILogger<ValidatedFileRepository> logger)
        {
            _basePath = Path.Combine(environment.WebRootPath, "images");
            _baseUrl = configuration["FileStorage:BaseUrl"] ?? "/images";
            _logger = logger;
            
            // Список разрешенных MIME-типов для изображений
            _allowedMimeTypes = new HashSet<string>
            {
                "image/jpeg",
                "image/jpg",
                "image/png",
                "image/gif",
                "image/bmp",
                "image/webp",
                "image/svg+xml"
            };
            
            if (!Directory.Exists(_basePath))
                Directory.CreateDirectory(_basePath);
                
            string profilesPath = Path.Combine(_basePath, "profiles");
            if (!Directory.Exists(profilesPath))
                Directory.CreateDirectory(profilesPath);
                
            string gamesPath = Path.Combine(_basePath, "games");
            if (!Directory.Exists(gamesPath))
                Directory.CreateDirectory(gamesPath);
                
            string productsPath = Path.Combine(_basePath, "products");
            if (!Directory.Exists(productsPath))
                Directory.CreateDirectory(productsPath);
        }

        public async Task<string> SaveProfileImageAsync(string userId, IFormFile file)
        {
            ValidateFile(file);
            return await SaveImage("profiles", userId, file);
        }
        
        public Task DeleteProfileImageAsync(string imageUrl)
        {
            return DeleteImage(imageUrl, "profiles");
        }

        public async Task<string> SaveGamePhotoAsync(string gameId, IFormFile file)
        {
            ValidateFile(file);
            return await SaveImage("games", gameId, file);
        }

        public Task DeleteGamePhotoAsync(string imageUrl)
        {
            return DeleteImage(imageUrl, "games");
        }

        public async Task<string> SaveProductImageAsync(string productId, IFormFile file)
        {
            ValidateFile(file);
            return await SaveImage("products", productId, file);
        }

        public Task DeleteProductImageAsync(string imageUrl)
        {
            return DeleteImage(imageUrl, "products");
        }

        private void ValidateFile(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                throw new ArgumentException("Файл не найден или пустой");
            }

            if (!_allowedMimeTypes.Contains(file.ContentType))
            {
                _logger.LogWarning($"Попытка загрузить файл с недопустимым MIME-типом: {file.ContentType}, Имя файла: {file.FileName}");
                throw new ArgumentException($"Недопустимый тип файла: {file.ContentType}. Разрешены только изображения.");
            }
        }

        private async Task<string> SaveImage(string folder, string Id, IFormFile file)
        {
            string fileName = $"{Id}_{DateTime.Now.Ticks}{Path.GetExtension(file.FileName)}";
            string filePath = Path.Combine(_basePath, folder, fileName);
            
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return $"{_baseUrl}/{folder}/{fileName}";
        }

        private Task DeleteImage(string imageUrl, string folder)
        {
            if (string.IsNullOrEmpty(imageUrl))
                return Task.CompletedTask;
                
            string fileName = Path.GetFileName(imageUrl);
            string filePath = Path.Combine(_basePath, folder, fileName);
            
            if (File.Exists(filePath))
                File.Delete(filePath);
            else
                throw new FileNotFoundException($"File {fileName} not found in {folder} folder");
                
            return Task.CompletedTask;
        }
    }
} 