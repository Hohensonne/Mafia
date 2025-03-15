using System.ComponentModel.DataAnnotations;

namespace Mafia.API.Contracts;

public record CancelRegistrationRequest([Required] string GameId);
