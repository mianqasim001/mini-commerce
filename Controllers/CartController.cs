using backend.Services;
using backend.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CartController : ControllerBase
    {
        private readonly CartService _cartService;

        public CartController(CartService cartService)
        {
            _cartService = cartService;
        }

        // GET: api/Cart/5
        [HttpGet("{userId}")]
        public async Task<IActionResult> GetCart(int userId)
        {
            var cart = await _cartService.GetCartDtoAsync(userId);
            return cart != null ? Ok(cart) : NotFound(new { message = "Cart is empty" });
        }

        // POST: api/Cart/add/5/10/1
        [HttpPost("add/{userId}/{productId}/{quantity}")]
        public async Task<IActionResult> Add(int userId, int productId, int quantity)
        {
            try {
                await _cartService.AddToCartAsync(userId, productId, quantity);
                return Ok(new { message = "Item added to cart" });
            } catch (Exception ex) {
                return BadRequest(new { message = ex.Message });
            }
        }

        // PUT: api/Cart/update/5/10/3 (Sets exact quantity)
        [HttpPut("update/{userId}/{productId}/{quantity}")]
        public async Task<IActionResult> UpdateQuantity(int userId, int productId, int quantity)
        {
            try {
                await _cartService.UpdateCartItemQuantityAsync(userId, productId, quantity);
                return Ok(new { message = "Quantity updated" });
            } catch (Exception ex) {
                return BadRequest(new { message = ex.Message });
            }
        }

        // DELETE: api/Cart/remove/5/10
        [HttpDelete("remove/{userId}/{productId}")]
        public async Task<IActionResult> Remove(int userId, int productId)
        {
            try {
                await _cartService.RemoveFromCartAsync(userId, productId);
                return Ok(new { message = "Item removed from cart" });
            } catch (Exception ex) {
                return BadRequest(new { message = ex.Message });
            }
        }

        // DELETE: api/Cart/clear/5
        [HttpDelete("clear/{userId}")]
        public async Task<IActionResult> Clear(int userId)
        {
            try {
                await _cartService.ClearCartAsync(userId);
                return Ok(new { message = "Cart cleared successfully" });
            } catch (Exception ex) {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}