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
    [ApiController]
    [Route("games")]
    public class GameController : ControllerBase
    {
        private readonly IGameService _gameService;

        public GameController(IGameService gameService)
        {
            _gameService = gameService;
        }

        [HttpGet("get-all")]
        public async Task<IActionResult> GetAllGames()
        {
            try
            {
                var games = await _gameService.GetAllGamesAsync();
                if (games == null)
                {
                    return NotFound();
                }
                return Ok(games);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("get-upcoming")]
        public async Task<IActionResult> GetUpcomingGames()
        {
            try
            {
                var games = await _gameService.GetUpcomingGamesAsync();
                if (games == null)
                {
                    return NotFound();
                }
                return Ok(games);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("get/{id}")]
        public async Task<IActionResult> GetGameById([FromRoute] string id)
        {
            try
            {
                var game = await _gameService.GetGameByIdAsync(id);
                if (game == null)
                {
                    return NotFound();
                }
                return Ok(game);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpGet("get-registered")]
        public async Task<IActionResult> GetRegisteredGames()
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var games = await _gameService.GetRegisteredGamesAsync(userId);
                if (games == null)
                {
                    return NotFound();
                }
                return Ok(games);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "Admin, Moderator")]
        [HttpPost("create")]
        public async Task<IActionResult> CreateGame(GameCreationRequest request)
        {
            try
            {
                var gameId = await _gameService.CreateGameAsync(request.Name, request.StartTime, request.EndOfRegistration, request.MaxPlayers);
                return Ok(gameId);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteGame(string id)
        {
            try
            {
                var gameId = await _gameService.DeleteGameAsync(id);
                return Ok(gameId);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [Authorize]
        [HttpPost("register/{gameId}")]
        public async Task<IActionResult> RegisterUserForGame([FromRoute] string gameId)
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var roles = User.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).ToList();
                Console.WriteLine(roles);
                var result = await _gameService.RegisterUserForGameAsync(gameId, userId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [Authorize]
        [HttpDelete("cancel-registration/{gameId}")]
        public async Task<IActionResult> CancelRegistration([FromRoute] string gameId)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            try
            {
                var result = await _gameService.CancelRegistrationAsync(gameId, userId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        
    }
}
