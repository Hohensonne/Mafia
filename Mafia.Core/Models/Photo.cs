using System;

namespace Mafia.Core.Models;

public class Photo
{
    public string Id { get; set; }
    public string ImageUrl { get; set; }
    public string GameId { get; set; }
    public string UserId { get; set; }
    public User User { get; set; }
    public Game Game { get; set; }
    public DateTime UploadedAt { get; set; }

}
