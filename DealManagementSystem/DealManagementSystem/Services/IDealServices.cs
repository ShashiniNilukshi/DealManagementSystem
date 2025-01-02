using DealManagementSystem.DTOs;

namespace DealManagementSystem.Services
{
    public interface IDealService
    {
        Task<List<DealDTO>> GetAllDealsAsync();
        Task<DealDTO?> GetDealByIdAsync(int id);
        Task<DealDTO> CreateDealAsync(DealDTO dealDto);
        Task<DealDTO?> UpdateDealAsync(int id, DealDTO dealDto);
        Task<bool> DeleteDealAsync(int id);
    }
}
