using Mafia.API.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mafia.Core.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Identity;

namespace Mafia.API.Controllers
{
    public class UsersController : ControllerBase
    {
        private readonly IUsersService _usersService;

        public UsersController(IUsersService usersService)
        {
            _usersService = usersService;
        }

        // GET: UserController/Create
        public async Task<ActionResult> Create(RegistrationRequest request)
        {
            await _usersService.Register(request.UserName, request.Email, request.Password);
            return Ok();
        }


        [HttpPost("login")]
        public async Task<ActionResult> Login([FromBody] LoginRequest request, HttpContext context)
        {
            try
            {
                var (JwtToken, RefreshToken) = await _usersService.Login(request.Email, request.Password);
                return Ok(new LoginResponse { JwtToken = JwtToken, RefreshToken = RefreshToken });
            }
            catch (InvalidOperationException)
            {
                return Unauthorized(new
                {
                    status = "error",
                    message = "Неверный логин или пароль",
                    details = "Пожалуйста, проверьте введенные данные или восстановите пароль."
                });
            }
        }

        //[HttpPost("refresh-token")]
        //public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenModel model)
        //{
        //    var user = await _userManager.Users.SingleOrDefaultAsync(u => u.RefreshToken == model.RefreshToken);

        //    if (user == null || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
        //        return Unauthorized();

        //    var roles = await _userManager.GetRolesAsync(user);
        //    var jwtToken = _tokenService.GenerateJwtToken(user, roles.ToList());
        //    var newRefreshToken = _tokenService.GenerateRefreshToken();

        //    user.RefreshToken = newRefreshToken;
        //    user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
        //    await _userManager.UpdateAsync(user);

        //    return Ok(new { JwtToken = jwtToken, RefreshToken = newRefreshToken });
        //}

    }
}
