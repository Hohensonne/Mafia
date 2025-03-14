using System.ComponentModel.DataAnnotations;

namespace Mafia.API.Contracts
{
    public record RegisterForTheGameRequest([Required] string GameId);
}