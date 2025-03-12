using System;

namespace Mafia.Core.Models;

public class GameRegistration
{
    public Guid Id { get; set; }
    public Guid GameId { get; set; }
    public Guid UserId { get; set; }
    public DateTime CreatedAt { get; set; }
    public Game Game { get; set; }
    public User User { get; set; }
    public bool IsApproved { get; set; }
}
