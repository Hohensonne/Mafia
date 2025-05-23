﻿using Mafia.API.Contracts;
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
                return Ok(new GetUserResponse(user.Id, user.FirstName, user.LastName, user.Email, user.PhoneNumber, user.ProfileImageUrl));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("get-all")]
        [Authorize(Roles = "Admin, Moderator")]
        public async Task<ActionResult> GetAll()
        {
            var users = await _usersService.GetAll();
            return Ok(users);
        }   


        [HttpPost("create")]
        public async Task<IActionResult> Create([FromForm] RegistrationRequest request)
        {
            try 
            {
                await _usersService.Register(request.FirstName, request.LastName, request.Email, request.PhoneNumber, request.Password, request.ProfileImage);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("update")]
        [Authorize]
        public async Task<IActionResult> Update([FromForm] UpdateUserRequest request)
        {
            try
            {
                var id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                await _usersService.Update(id, request.FirstName, request.LastName, request.Email, request.PhoneNumber, request.Password);
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
                var (jwtToken, refreshToken, roles) = await _usersService.Login(request.Email, request.Password);
                return Ok(new LoginResponse(jwtToken, refreshToken, roles));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("update-profile-image")]
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
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest request)
        {
            try
            {
                (string newJwtToken, string newRefreshToken, IEnumerable<string> roles) = await _usersService.RefreshTokenAsync(request.RefreshToken);
                return Ok(new LoginResponse(newJwtToken, newRefreshToken, roles));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
