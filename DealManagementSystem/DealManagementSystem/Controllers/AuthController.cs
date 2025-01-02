using DealManagementSystem.DTOs.Auth;
using DealManagementSystem.DTOs;
using DealManagementSystem.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using System.Security.Claims;

namespace DealManagementSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ILogger<AuthController> _logger;

        public AuthController(IAuthService authService, ILogger<AuthController> logger)
        {
            _authService = authService;
            _logger = logger;
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<ActionResult<AuthResponseDTO>> Login([FromBody] LoginDTO model)
        {
            try
            {
                _logger.LogInformation("Login attempt for email: {Email}", model.Email);
                var result = await _authService.LoginAsync(model);
                _logger.LogInformation("Login successful for email: {Email}", model.Email);
                return Ok(result);
            }
            catch (UnauthorizedAccessException ex)
            {
                _logger.LogWarning("Login failed for email: {Email}. Error: {Error}", model.Email, ex.Message);
                return Unauthorized(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred during login for email: {Email}", model.Email);
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpPost("register")]
        [AllowAnonymous] // Allow all users to register
        public async Task<ActionResult<AuthResponseDTO>> Register([FromBody] RegisterUserDTO model)
        {
            try
            {
                _logger.LogInformation("Registration attempt for email: {Email}", model.Email);
                var result = await _authService.RegisterAsync(model);
                _logger.LogInformation("Registration successful for email: {Email}", model.Email);
                return Ok(result);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning("Registration failed for email: {Email}. Error: {Error}", model.Email, ex.Message);
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred during registration for email: {Email}", model.Email);
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpPost("change-password")]
        [Authorize]
        public async Task<IActionResult> ChangePassword(string currentPassword, string newPassword)
        {
            try
            {
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
                _logger.LogInformation("User with ID {UserId} is attempting to change their password.", userId);
                await _authService.ChangePasswordAsync(userId, currentPassword, newPassword);
                _logger.LogInformation("Password changed successfully for user with ID {UserId}.", userId);
                return Ok(new { message = "Password changed successfully" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while changing the password for user with ID {UserId}.", User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}