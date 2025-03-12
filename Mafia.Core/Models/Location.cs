using System;

namespace Mafia.Core.Models;

public class Location
{
    public Guid Id { get; set; }
    public ICollection<Game> Games { get; set; }
}
