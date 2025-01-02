using DealManagementSystem.DTOs;
using DealManagementSystem.Interfaces;
using DealManagementSystem.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DealManagementSystem.Services
{
    public class ItineraryService : IItineraryService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ItineraryService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<ItineraryDTO>> GetItinerariesByDealIdAsync(int dealId)
        {
            var deal = await _unitOfWork.Deals
                .Include(d => d.Itineraries) // Eagerly load the related Itineraries
                .FirstOrDefaultAsync(d => d.Id == dealId);

            if (deal == null) return null;

            return deal.Itineraries.Select(i => new ItineraryDTO
            {
                Name = i.Name,
                Day = i.Day
            }).ToList();
        }

        public async Task<ItineraryDTO> AddItineraryToDealAsync(int dealId, AddItineraryToDealDTO itineraryDto)
        {
            var deal = await _unitOfWork.Deals
                .Include(d => d.Itineraries) // Eagerly load the related Itineraries
                .FirstOrDefaultAsync(d => d.Id == dealId);

            if (deal == null) return null;

            var itinerary = new Itinerary
            {
                Name = itineraryDto.Itinerary.Name,
                Day = itineraryDto.Itinerary.Day,
                CreatedAt = DateTime.UtcNow
            };

            deal.Itineraries.Add(itinerary);
            await _unitOfWork.Deals.UpdateAsync(deal);
            await _unitOfWork.CompleteAsync();

            return new ItineraryDTO
            {
                Name = itinerary.Name,
                Day = itinerary.Day
            };
        }

        public async Task<ItineraryDTO> UpdateItineraryAsync(int itineraryId, ItineraryDTO itineraryDto)
        {
            var itinerary = await _unitOfWork.Itineraries.GetByIdAsync(itineraryId);
            if (itinerary == null) return null;

            itinerary.Name = itineraryDto.Name;
            itinerary.Day = itineraryDto.Day;
            itinerary.UpdatedAt = DateTime.UtcNow;

            await _unitOfWork.Itineraries.UpdateAsync(itinerary);
            await _unitOfWork.CompleteAsync();

            return new ItineraryDTO
            {
                Name = itinerary.Name,
                Day = itinerary.Day
            };
        }

        public async Task<bool> DeleteItineraryAsync(int itineraryId)
        {
            var itinerary = await _unitOfWork.Itineraries.GetByIdAsync(itineraryId);
            if (itinerary == null) return false;

            await _unitOfWork.Itineraries.DeleteAsync(itinerary);
            await _unitOfWork.CompleteAsync();

            return true;
        }

   
    }
}