using Repository;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Models;
using Dtos;

namespace Mafia.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly JwtRepository _jwtRepository;

        public AuthController(UserManager<User> userManager, RoleManager<IdentityRole> roleManager, JwtRepository jwtRepository)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _jwtRepository = jwtRepository;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegistrationDto dto)
        {
            var user = new User { UserName = dto.Email, Email = dto.Email };
            var result = await _userManager.CreateAsync(user, dto.Password);

            if (!result.Succeeded)
                return BadRequest(result.Errors);

            if (!await _roleManager.RoleExistsAsync("user"))
                await _roleManager.CreateAsync(new IdentityRole("user"));

            await _userManager.AddToRoleAsync(user, "user");

            return Ok(new { Message = "User registered successfully" });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            var user = await _userManager.FindByEmailAsync(dto.Email);
            if (user == null || !await _userManager.CheckPasswordAsync(user, dto.Password))
                return Unauthorized();

            var roles = await _userManager.GetRolesAsync(user);

            var jwtToken = _jwtRepository.GenerateJwtToken(user, roles.ToList());
            var refreshToken = _jwtRepository.GenerateRefreshToken();

            // Сохраните refresh token в базу данных или кэш
            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
            await _userManager.UpdateAsync(user);

            return Ok(new { JwtToken = jwtToken, RefreshToken = refreshToken });
        }
    }
}

