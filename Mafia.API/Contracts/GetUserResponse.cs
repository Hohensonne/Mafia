namespace Mafia.API.Contracts
{
    public record GetUserResponse(string Id, string FirstName, string LastName, string Email, string ProfileImageUrl);
}
