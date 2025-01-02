using DealManagementSystem.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DealManagementSystem.Services
{
    public interface IMediaService
    {
        Task<List<MediaDTO>> GetMediaByHotelIdAsync(int hotelId);
        Task<MediaDTO> AddHotelMediaAsync(int hotelId, AddHotelMediaDTO mediaDto);
        Task<MediaDTO> UpdateHotelMediaAsync(int mediaId, UpdateHotelMediaDTO mediaDto);
        Task<bool> DeleteHotelMediaAsync(int mediaId);

        // Add these methods
        Task<List<MediaDTO>> GetMediaByDealIdAsync(int dealId);
        Task<MediaDTO> AddMediaToDealAsync(int dealId, AddMediaToDealDTO mediaDto);
        Task<MediaDTO> UpdateMediaAsync(int mediaId, MediaDTO mediaDto);
        Task<bool> DeleteMediaAsync(int mediaId);
    }
}