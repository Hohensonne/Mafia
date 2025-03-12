using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Mafia.Core.Interfaces;

namespace Mafia.Infrastructre;


public class FileRepository : IFileRepository
{
    private readonly string _basePath;
    private readonly string _baseUrl;

    public FileRepository(IWebHostEnvironment environment, IConfiguration configuration)
    {
        _basePath = Path.Combine(environment.WebRootPath, "images", "profiles");
        _baseUrl = configuration["FileStorage:BaseUrl"] ?? "/images/profiles";
        
        if (!Directory.Exists(_basePath))
            Directory.CreateDirectory(_basePath);
    }

    public async Task<string> SaveProfileImageAsync(string userId, IFormFile file)
    {
        // Генерируем уникальное имя файла
        string uniqueFileName = $"{userId}_{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
        string filePath = Path.Combine(_basePath, uniqueFileName);
        
        // Сохраняем файл
        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }
        
        return $"{_baseUrl}/{uniqueFileName}";
    }

    public Task DeleteProfileImageAsync(string imageUrl)
    {
        if (string.IsNullOrEmpty(imageUrl))
            return Task.CompletedTask;
            
        string fileName = Path.GetFileName(imageUrl);
        string filePath = Path.Combine(_basePath, fileName);
        
        if (File.Exists(filePath))
            File.Delete(filePath);
            
        return Task.CompletedTask;
    }
}