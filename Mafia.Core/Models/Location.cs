using System;

namespace Mafia.Core.Models;

public class Location
{
    public string Id { get; set; }
    public ICollection<Game> Games { get; set; }
}
