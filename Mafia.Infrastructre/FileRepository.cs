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
        _basePath = Path.Combine(environment.WebRootPath, "images");
        _baseUrl = configuration["FileStorage:BaseUrl"] ?? "/images";
        
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
        return await SaveImage("profiles", userId, file);
    }
    
    public Task DeleteProfileImageAsync(string imageUrl)
    {
        return DeleteImage(imageUrl, "profiles");
    }

    public async Task<string> SaveGamePhotoAsync(string gameId, IFormFile file)
    {
        return await SaveImage("games", gameId, file);
    }

    public Task DeleteGamePhotoAsync(string imageUrl)
    {
        return DeleteImage(imageUrl, "games");
    }

    public async Task<string> SaveProductImageAsync(string productId, IFormFile file)
    {
        return await SaveImage("products", productId, file);
    }

    public Task DeleteProductImageAsync(string imageUrl)
    {
        return DeleteImage(imageUrl, "products");
    }


    private async Task<string> SaveImage(string folder, string Id, IFormFile file)
    {
        if (!file.ContentType.StartsWith("image/"))
            throw new ArgumentException("File is not an image");

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