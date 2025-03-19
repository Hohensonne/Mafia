using System;
using Microsoft.AspNetCore.Mvc;
using Mafia.Application.Services;
using Mafia.Core.Models;
using Mafia.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
namespace Mafia.API.Controllers
{
    [ApiController]
    [Route("orders")]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet("get-all")]
        [Authorize(Roles = "Admin, Moderator")]
        public async Task<IActionResult> GetAllOrders()
        {
            try
            {
                var orders = await _orderService.GetAllOrdersAsync();
                if (orders == null)
                {
                    return NotFound();
                }
                return Ok(orders);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("create")]
        [Authorize]
        public async Task<IActionResult> CreateOrder()
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var orderId = await _orderService.CreateOrderFromCartAsync(userId);
                return Ok(orderId);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("get")]
        [Authorize]
        public async Task<IActionResult> GetOrders()
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var orders = await _orderService.GetUserOrdersAsync(userId);
                if (orders == null)
                {
                    return NotFound();
                }
                return Ok(orders);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("get/{orderId}")]
        [Authorize]
        public async Task<IActionResult> GetOrderById([FromRoute] string orderId)
        {
            try
            {
                var order = await _orderService.GetOrderByIdAsync(orderId);
                if (order == null)
                {
                    return NotFound();
                }
                return Ok(order);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("cancel-force/{orderId}")]
        [Authorize(Roles = "Admin, Moderator")]
        public async Task<IActionResult> CancelOrderForce([FromRoute] string orderId)
        {
            try
            {
                await _orderService.CancelOrderAsync(orderId);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("cancel/{orderId}")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> CancelOrder([FromRoute] string orderId)
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                await _orderService.CancelOrderAsync(orderId, userId);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("update-status/{orderId}")]
        [Authorize(Roles = "Admin, Moderator")]
        public async Task<IActionResult> UpdateOrderStatus([FromRoute] string orderId, [FromBody] string status)
        {
            try
            {
                await _orderService.UpdateOrderStatusAsync(orderId, status);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}