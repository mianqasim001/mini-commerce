using backend.Services;
using backend.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Serialization;

namespace backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly OrderService _orderService;
        private readonly OrderRepository _orderRepo;

        public OrderController(OrderService orderService, OrderRepository orderRepo)
        {
            _orderService = orderService;
            _orderRepo = orderRepo;
        }

        // POST: api/Order/checkout/5
        [HttpPost("checkout/{userId}")]
        public async Task<IActionResult> Checkout(int userId, [FromBody] CheckoutRequest request)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(request.Address))
                    return BadRequest(new { message = "Shipping address is required." });

                var order = await _orderService.CheckoutAsync(userId, request.Address);
                return Ok(new { message = "Order placed successfully", orderId = order.OrderId });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // GET: api/Order/history/5
        [HttpGet("history/{userId}")]
        public async Task<IActionResult> GetHistory(int userId)
        {
            try
            {
                var orders = await _orderRepo.GetOrdersByUserIdAsync(userId);
                return Ok(orders);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // GET: api/Order/details/10
        [HttpGet("details/{orderId}")]
        public async Task<IActionResult> GetDetails(int orderId)
        {
            var order = await _orderRepo.GetOrderByIdAsync(orderId);
            if (order == null) return NotFound(new { message = "Order not found" });
            return Ok(order);
        }

        // PUT: api/Order/cancel/10/user/5
        [HttpPut("cancel/{orderId}/user/{userId}")]
        public async Task<IActionResult> CancelOrder(int orderId, int userId)
        {
            try
            {
                await _orderService.CancelOrderAsync(orderId, userId);
                return Ok(new { message = "Order cancelled successfully and stock restored." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }

    // DTO for receiving the address from Frontend JSON body
    public class CheckoutRequest 
    { 
        public string Address { get; set; } = string.Empty; 
    }
}