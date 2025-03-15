using System;

namespace Mafia.API.Contracts;

public class GameCreationRequest
{
    public string Name { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndOfRegistration { get; set; }
    public int MaxPlayers { get; set; }
}
