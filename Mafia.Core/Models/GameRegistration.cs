namespace Mafia.Core.Models;

public class GameRegistration
{
    public string Id { get; set; }
    public string GameId { get; set; }
    public string UserId { get; set; }
    public DateTime CreatedAt { get; set; }
    public Game Game { get; set; }
    public User User { get; set; }
    public bool IsApproved { get; set; }
}
