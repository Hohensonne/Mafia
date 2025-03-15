using System.ComponentModel.DataAnnotations;

namespace Mafia.API.Contracts
{
    public record RegisterUserForGameRequest([Required] string GameId);
}