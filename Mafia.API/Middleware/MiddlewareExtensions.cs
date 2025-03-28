using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;

namespace Mafia.API.Middleware
{
    public static class MiddlewareExtensions
    {
        public static IServiceCollection AddFileValidation(this IServiceCollection services)
        {
            // Список разрешенных MIME-типов для изображений
            var allowedMimeTypes = new List<string>
            {
                "image/jpeg",
                "image/jpg",
                "image/png",
                "image/gif",
                "image/bmp",
                "image/webp",
                "image/svg+xml"
            };

            services.AddSingleton<IFileValidator>(new MimeTypeValidator(allowedMimeTypes));
            return services;
        }

        public static IApplicationBuilder UseFileValidation(this IApplicationBuilder app)
        {
            app.UseMiddleware<FileValidationMiddleware>();
            return app;
        }
    }
} 