using DealManagementSystem.DTOs;

namespace DealManagementSystem.Services
{
    public interface IHotelService
    {
        Task<List<HotelDTO>> GetHotelsByDealIdAsync(int dealId);
        Task<HotelDTO> AddHotelToDealAsync(int dealId, HotelDTO hotelDto);
        Task<HotelDTO> UpdateHotelAsync(int hotelId, HotelDTO hotelDto);
        Task<bool> DeleteHotelAsync(int hotelId);

        // For updating media for hotels (images/videos)
        Task<List<MediaDTO>> AddMediaToHotelAsync(int hotelId, List<AddHotelMediaDTO> mediaDtos);
        Task<bool> UpdateMediaForHotelAsync(int mediaId, UpdateHotelMediaDTO mediaDto);
        Task<bool> DeleteHotelMediaAsync(int mediaId);
     
    }
}
