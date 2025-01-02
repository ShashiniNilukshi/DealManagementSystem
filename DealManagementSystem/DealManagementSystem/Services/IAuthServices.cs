// Services/IAuthService.cs
using DealManagementSystem.DTOs.Auth;
using DealManagementSystem.DTOs;

namespace DealManagementSystem.Services
{
    public interface IAuthService
    {
        Task<AuthResponseDTO> LoginAsync(LoginDTO model);
        Task<AuthResponseDTO> RegisterAsync(RegisterUserDTO model);
        Task<bool> ChangePasswordAsync(int userId, string currentPassword, string newPassword);
    }
}