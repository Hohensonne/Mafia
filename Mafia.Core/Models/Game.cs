using System;

namespace Mafia.Core.Models;

public class Game
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndOfRegistration { get; set; }
    public Location Location { get; set; }
    public int MaxPlayers { get; set; }
    public int CurrentPlayers { get; set; }
    public DateTime CreatedAt { get; set; }    
}
