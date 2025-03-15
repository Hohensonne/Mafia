using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mafia.Core.Interfaces;
using Mafia.Core.Models;
using Mafia.API.Contracts;
using Microsoft.AspNetCore.Authorization;
namespace Mafia.API.Controllers
{
    [Route("products")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet("get-all")]
        public async Task<IActionResult> GetAllProducts()
        {
            try
            {
                var products = await _productService.GetAllProductsAsync();
                if (products == null)
                {
                    return NotFound();
                }
                return Ok(products);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("get/{id}")]
        public async Task<IActionResult> GetProductById([FromRoute] string id)
        {
            try
            {
                var product = await _productService.GetProductByIdAsync(id);
                if (product == null)
                {
                    return NotFound();
                }
                return Ok(product);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "Admin, Moderator")]
        [HttpPost("create")]
        public async Task<IActionResult> CreateProduct([FromForm] CreateProductRequest request)
        {
            try
            {
                var productId = await _productService.CreateProductAsync(request.Name, request.Description, request.Price, request.AvailableQuantity, request.Category, request.Image);
                return Ok(productId);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "Admin, Moderator")]
        [HttpPut("update/{id}")]
        public async Task<IActionResult> UpdateProduct([FromForm] UpdateProductRequest request)
        {
            try
            {
                var product = await _productService.UpdateProductAsync(request.Id, request.Name, request.Description, request.Price, request.AvailableQuantity, request.Category, request.Image);
                return Ok(product);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "Admin, Moderator")]
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteProduct([FromRoute] string id)
        {
            try
            {
                var product = await _productService.DeleteProductAsync(id);
                return Ok(product);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "Admin, Moderator")]
        [HttpPut("updateStock/{id}")]
        public async Task<IActionResult> UpdateProductStock([FromRoute] string id, [FromBody] int quantity)
        {
            try
            {
                var product = await _productService.UpdateProductStockAsync(id, quantity);
                return Ok(product);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
