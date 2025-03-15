using System;
using Microsoft.AspNetCore.Mvc;
using Mafia.Application.Services;
using Mafia.Core.Models;
using Mafia.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Mafia.API.Contracts;
namespace Mafia.API.Controllers
{
    [ApiController]
    [Route("cart")]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;

        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }
        [HttpGet("get")]
        [Authorize]
        public async Task<IActionResult> GetCart()
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var cart = await _cartService.GetUserCartAsync(userId);
                if (cart == null)
                {
                    return NotFound();
                }
                return Ok(cart);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("update")]
        [Authorize]
        public async Task<IActionResult> UpdateCart(UpdateCartRequest request)
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var result = await _cartService.UpdateCartItemAsync(userId, request.ProductId, request.Quantity);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("clear")]
        [Authorize]
        public async Task<IActionResult> ClearCart()
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                await _cartService.ClearCartAsync(userId);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}