using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Mafia.API.Middleware
{
    public class FileValidationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IFileValidator _fileValidator;
        private readonly ILogger<FileValidationMiddleware> _logger;

        public FileValidationMiddleware(
            RequestDelegate next, 
            IFileValidator fileValidator,
            ILogger<FileValidationMiddleware> logger)
        {
            _next = next;
            _fileValidator = fileValidator;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Проверяем только запросы с файлами
            if (context.Request.HasFormContentType && context.Request.Form.Files.Any())
            {
                foreach (var file in context.Request.Form.Files)
                {
                    if (!_fileValidator.IsValid(file))
                    {
                        _logger.LogWarning($"Отклонен файл: {file.FileName}, MIME-тип: {file.ContentType}");
                        
                        context.Response.StatusCode = StatusCodes.Status415UnsupportedMediaType;
                        await context.Response.WriteAsync($"Недопустимый тип файла: {file.ContentType}. Допускаются только изображения.");
                        return;
                    }
                }
            }

            // Передаем управление следующему middleware
            await _next(context);
        }
    }
} 