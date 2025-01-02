using DealManagementSystem.DTOs;

namespace DealManagementSystem.Services
{
    public interface IItineraryService
    {
        Task<List<ItineraryDTO>> GetItinerariesByDealIdAsync(int dealId);
        Task<ItineraryDTO> AddItineraryToDealAsync(int dealId, AddItineraryToDealDTO itineraryDto);
        Task<ItineraryDTO> UpdateItineraryAsync(int itineraryId, ItineraryDTO itineraryDto);
        Task<bool> DeleteItineraryAsync(int itineraryId);
       
    }
}

  

