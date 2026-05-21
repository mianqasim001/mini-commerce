using backend.DTOs;
using backend.Services;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;

        public AuthController(AuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto dto)
        {
            try
            {
                var user = await _authService.RegisterAsync(dto);
                var token = await _authService.LoginAsync(new LoginDto { Email = dto.Email, Password = dto.Password });
                return Ok(new { 
                    token, 
                    user = new { id = user.UserId, name = user.Name, email = user.Email, phone = user.Phone, address = user.Address },
                    message = "Registration successful" 
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            try
            {
                var user = await _authService.LoginUserAsync(dto);
                var token = await _authService.LoginAsync(dto);
                return Ok(new { 
                    token, 
                    user = new { id = user.UserId, name = user.Name, email = user.Email, phone = user.Phone, address = user.Address },
                    message = "Login successful" 
                });
            }
            catch (Exception ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
        }

        [Authorize]
        [HttpGet("profile")]
        public async Task<IActionResult> GetProfile()
        {
            try
            {
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);
                var user = await _authService.GetUserByIdAsync(userId);
                return Ok(new {
                    id = user.UserId,
                    name = user.Name,
                    email = user.Email,
                    phone = user.Phone,
                    address = user.Address
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [Authorize]
        [HttpPut("update")]
        public async Task<IActionResult> UpdateAccount(UserUpdateDto dto)
        {
            try
            {
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);
                await _authService.UpdateUserAsync(userId, dto);
                var updatedUser = await _authService.GetUserByIdAsync(userId);
                return Ok(new { 
                    message = "Account updated successfully",
                    user = new { id = updatedUser.UserId, name = updatedUser.Name, email = updatedUser.Email, phone = updatedUser.Phone, address = updatedUser.Address }
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

[Authorize]
[HttpDelete("delete")]
public async Task<IActionResult> DeleteAccount() // <-- Removed 'int userId' from here
{
    try
    {
        // Extract the UserId directly from the JWT Token claims safely
        var userIdStr = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userIdStr))
        {
            return Unauthorized(new { message = "Invalid token claims." });
        }

        int userId = int.Parse(userIdStr);
        
        await _authService.DeleteUserAsync(userId);
        return Ok(new { message = "Account deleted successfully" });
    }
    catch (Exception ex)
    {
        return BadRequest(new { message = ex.Message });
    }
}
    }
}