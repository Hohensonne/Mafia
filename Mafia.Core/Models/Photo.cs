using System;

namespace Mafia.Core.Models;

public class Photo
{
    public Guid Id { get; set; }
    public string Url { get; set; }
    public Guid GameId { get; set; }
    public string UserId { get; set; }
    public User User { get; set; }
    public Game Game { get; set; }
    public DateTime UploadedAt { get; set; }

}
