using System;

namespace Mafia.API.Middleware
{
    public class InvalidFileException : Exception
    {
        public InvalidFileException() : base("Недопустимый тип файла")
        {
        }

        public InvalidFileException(string message) : base(message)
        {
        }

        public InvalidFileException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
} 