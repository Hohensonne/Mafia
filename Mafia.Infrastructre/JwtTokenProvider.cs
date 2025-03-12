using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Mafia.Core.Models;
using Mafia.Core.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Mafia.Infrastructre
{
    public class JwtTokenProvider : IJwtTokenProvider
    {
        private readonly IConfiguration _configuration;
        private readonly UserManager<User> _userManager;

        public JwtTokenProvider(IConfiguration configuration, UserManager<User> userManager)
        {
            _configuration = configuration;
            _userManager = userManager;
        }

        public string GenerateJwtToken(User user, IList<string> roles)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Email, user.Email!)
            };

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using var rng = System.Security.Cryptography.RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        public async Task SaveRefreshTokenAsync(User user)
        {
            await _userManager.Users
                .Where(x => x.Id == user.Id)
                .ExecuteUpdateAsync(s => s
                    .SetProperty(x => x.RefreshToken, user.RefreshToken)
                    .SetProperty(x => x.RefreshTokenExpiryTime, user.RefreshTokenExpiryTime)
                );
        }

        public async Task<User?> GetRefreshTokenAsync(string refreshToken)
        {
            return await _userManager.Users.FirstOrDefaultAsync(u => u.RefreshToken == refreshToken);
        }

        public async Task RemoveRefreshTokenAsync(User user)
        {
            await _userManager.Users
                .Where(x => x.Id == user.Id)
                .ExecuteUpdateAsync(s => s
                    .SetProperty(x => x.RefreshToken, "")
                    .SetProperty(x => x.RefreshTokenExpiryTime, DateTime.MinValue)
                );
        }
    }
}
