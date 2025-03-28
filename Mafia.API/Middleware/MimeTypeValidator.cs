using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Linq;

namespace Mafia.API.Middleware
{
    public class MimeTypeValidator : IFileValidator
    {
        private readonly HashSet<string> _allowedMimeTypes;

        public MimeTypeValidator(IEnumerable<string> allowedMimeTypes)
        {
            _allowedMimeTypes = new HashSet<string>(allowedMimeTypes);
        }

        public bool IsValid(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return false;
            }

            // Проверяем MIME-тип
            return _allowedMimeTypes.Contains(file.ContentType);
        }
    }
} 