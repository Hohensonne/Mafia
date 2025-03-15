using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mafia.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Mafia.API.Contracts;


namespace Mafia.API.Controllers
{
    [Route("photos")]
    [ApiController]
    public class PhotoController : ControllerBase
    {
        private readonly IPhotoService _photoService;

        public PhotoController(IPhotoService photoService)
        {
            _photoService = photoService;
        }

        [HttpGet("get-all")]
        public async Task<IActionResult> GetAllPhotos()
        {
            try
            {
                var photos = await _photoService.GetAllPhotosAsync();
                if (photos == null)
                {
                    return NotFound();
                }
                return Ok(photos);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("get-by-game/{gameId}")]
        public async Task<IActionResult> GetPhotosByGame([FromRoute] string gameId)
        {
            try
            {
                var photos = await _photoService.GetPhotosByGameAsync(gameId);
                if (photos == null)
                {
                    return NotFound();
                }
                return Ok(photos);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("get-by-user/{userId}")]
        public async Task<IActionResult> GetPhotosByUser([FromRoute] string userId)
        {
            try
            {
                var photos = await _photoService.GetPhotosByUserAsync(userId);
                if (photos == null)
                {
                    return NotFound();
                }
                return Ok(photos);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("get/{photoId}")]
        public async Task<IActionResult> GetPhotoById([FromRoute] string photoId)
        {
            try
            {
                var photo = await _photoService.GetPhotoByIdAsync(photoId);
                if (photo == null)
                {
                    return NotFound();
                }
                return Ok(photo);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        
        [Authorize]
        [HttpPost("add/{gameId}")]
        public async Task<IActionResult> AddPhotoToGame([FromRoute] string gameId, [FromForm] AddPhotoRequest request)
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var result = await _photoService.AddPhotoToGameAsync(gameId, userId, request.File);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpDelete("delete/{photoId}")]
        public async Task<IActionResult> DeletePhoto([FromRoute] string photoId)
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var result = await _photoService.DeletePhotoAsync(photoId, userId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
