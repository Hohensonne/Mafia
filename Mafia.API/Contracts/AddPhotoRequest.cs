using Microsoft.AspNetCore.Http;

namespace Mafia.API.Contracts;

public record class AddPhotoRequest
{
    public IFormFile File { get; set; }
}
