using Mafia.API.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mafia.Core.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.VisualBasic;
namespace Mafia.API.Controllers
{
    [Route("user")]
    [ApiController]
    [Produces("application/json")]
    public class UsersController : ControllerBase
    {
        private readonly IUsersService _usersService;

        public UsersController(IUsersService usersService)
        {
            _usersService = usersService;
        }


        [Authorize]
        [HttpGet("get")]
        public async Task<IActionResult> Get()
        {
            try
            {
                var email = User.FindFirst(ClaimTypes.Email)?.Value;
                var user = await _usersService.GetByEmail(email);
                return Ok(new GetUserResponse(user.Id, user.UserName, user.Email, user.ProfileImageUrl));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //[HttpGet("getall")]
        //public async Task<ActionResult> GetAll()
        //{
        //    var users = await _usersService.GetAll();
        //    return Ok(users);
        //}   


        [HttpPost("create")]
        public async Task<IActionResult> Create([FromForm] RegistrationRequest request)
        {
            try 
            {
                await _usersService.Register(request.UserName, request.Email, request.Password, request.ProfileImage);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            try
            {
                var (jwtToken, refreshToken) = await _usersService.Login(request.Email, request.Password);
                return Ok(new LoginResponse(jwtToken, refreshToken));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("updateProfileImage")]
        [Authorize]
        public async Task<IActionResult> UpdateProfileImage([FromForm] UpdateProfileImageRequest request)
        {
            try
            {
                var email = User.FindFirst(ClaimTypes.Email)?.Value;
                await _usersService.UpdateProfileImage(email, request.ProfileImage);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        
        

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken([FromBody] string refreshToken)
        {
            try
            {
                var (newJwtToken, newRefreshToken) = await _usersService.RefreshTokenAsync(refreshToken);
                return Ok(new LoginResponse(newJwtToken, newRefreshToken));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
