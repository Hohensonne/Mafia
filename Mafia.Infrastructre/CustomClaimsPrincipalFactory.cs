using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Mafia.Core.Models;
using System.Security.Claims;

namespace Mafia.Infrastructre;

public sealed class CustomClaimsPrincipalFactory : IUserClaimsPrincipalFactory<User>
{
    public IdentityOptions Options { get; }

    public UserManager<User> UserManager { get; }

    public CustomClaimsPrincipalFactory(IOptions<IdentityOptions> options, UserManager<User> userManager)
    {
        Options = options.Value;
        UserManager = userManager;
    }

    public async Task<ClaimsPrincipal> CreateAsync(User user)
    {
        var claims = await GenerateClaimsAsync(user);
        return new ClaimsPrincipal(claims);
    }

    private async Task<ClaimsIdentity> GenerateClaimsAsync(User user)
    {
        var userId = user.Id.ToString();
        var claims = new ClaimsIdentity(
            IdentityConstants.ApplicationScheme,
            Options.ClaimsIdentity.UserNameClaimType,
            Options.ClaimsIdentity.RoleClaimType);

        claims.AddClaim(new Claim(Options.ClaimsIdentity.UserIdClaimType, userId));
        if (user.UserName is not null)
        {
            claims.AddClaim(new Claim(Options.ClaimsIdentity.UserNameClaimType, user.UserName));
        }

        if (UserManager.SupportsUserSecurityStamp)
        {
            claims.AddClaim(new Claim(
                Options.ClaimsIdentity.SecurityStampClaimType,
                await UserManager.GetSecurityStampAsync(user)));
        }

        if (UserManager.SupportsUserClaim)
        {
            claims.AddClaims(await UserManager.GetClaimsAsync(user));
        }

        return claims;
    }
}