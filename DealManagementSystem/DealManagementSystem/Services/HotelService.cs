using DealManagementSystem.DTOs;
using DealManagementSystem.Interfaces;
using DealManagementSystem.Models;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DealManagementSystem.Services
{
    public class HotelService : IHotelService
    {
        private readonly IUnitOfWork _unitOfWork;

        public HotelService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<HotelDTO>> GetHotelsByDealIdAsync(int dealId)
        {
            var deal = await _unitOfWork.Deals
                .Include(d => d.Hotels) // Eagerly load the related Hotels
                .FirstOrDefaultAsync(d => d.Id == dealId);

            if (deal == null) return null;

            return deal.Hotels.Select(h => new HotelDTO
            {
                Name = h.Name,
                Rate = h.Rate,
                Amenities = h.Amenities
            }).ToList();
        }


        public async Task<HotelDTO> AddHotelToDealAsync(int dealId, HotelDTO hotelDto)
        {
            var deal = await _unitOfWork.Deals
                .Include(d => d.Hotels) // Eagerly load the related Hotels
                .Include(d => d.Itineraries) // Eagerly load the related Itineraries if needed
                .FirstOrDefaultAsync(d => d.Id == dealId);

            if (deal == null) return null;

            // Ensure the Hotels collection is initialized if it's null
            if (deal.Hotels == null)
            {
                deal.Hotels = new List<Hotel>();
            }

            var hotel = new Hotel
            {
                Name = hotelDto.Name,
                Rate = hotelDto.Rate,
                Amenities = hotelDto.Amenities,
                CreatedAt = DateTime.UtcNow
            };

            // Add the new hotel to the deal's hotels collection
            deal.Hotels.Add(hotel);

            await _unitOfWork.Deals.UpdateAsync(deal);
            await _unitOfWork.CompleteAsync();

            return new HotelDTO
            {
                Name = hotel.Name,
                Rate = hotel.Rate,
                Amenities = hotel.Amenities
            };
        }

        public async Task<HotelDTO> UpdateHotelAsync(int hotelId, HotelDTO hotelDto)
        {
            var hotel = await _unitOfWork.Hotels.GetByIdAsync(hotelId);
            if (hotel == null) return null;

            hotel.Name = hotelDto.Name;
            hotel.Rate = hotelDto.Rate;
            hotel.Amenities = hotelDto.Amenities;
            hotel.UpdatedAt = DateTime.UtcNow;

            await _unitOfWork.Hotels.UpdateAsync(hotel);
            await _unitOfWork.CompleteAsync();

            return new HotelDTO
            {
                Name = hotel.Name,
                Rate = hotel.Rate,
                Amenities = hotel.Amenities
            };
        }

        public async Task<bool> DeleteHotelAsync(int hotelId)
        {
            var hotel = await _unitOfWork.Hotels.GetByIdAsync(hotelId);
            if (hotel == null) return false;

            await _unitOfWork.Hotels.DeleteAsync(hotel);
            await _unitOfWork.CompleteAsync();

            return true;
        }

        // Add media (images/videos) to hotel
        public async Task<List<MediaDTO>> AddMediaToHotelAsync(int hotelId, List<AddHotelMediaDTO> mediaDtos)
        {
            var hotel = await _unitOfWork.Hotels.GetByIdAsync(hotelId);
            if (hotel == null) return null;

            var mediaList = new List<Media>();
            foreach (var mediaDto in mediaDtos)
            {
                var media = new Media
                {
                    HotelId = hotelId, // Associate the media with the hotel
                    //Type = mediaDto.Media.Type, // Get media type (e.g., "Video")
                    Url = mediaDto.Media.URL, // Get media URL
                    CreatedAt = DateTime.UtcNow // Set the creation timestamp
                };

                hotel.Media.Add(media);
                mediaList.Add(media);
            }

            await _unitOfWork.Hotels.UpdateAsync(hotel);
            await _unitOfWork.CompleteAsync();

            return mediaList.Select(m => new MediaDTO
            {
                //Type = (int)m.Type,
                URL = m.Url
            }).ToList();
        }

        public async Task<bool> UpdateMediaForHotelAsync(int mediaId, UpdateHotelMediaDTO mediaDto)
        {
            var media = await _unitOfWork.Media.GetByIdAsync(mediaId);
            if (media == null) return false;

            foreach (var mediaItem in mediaDto.Media)
            {
                // Update the URL of the media item
                media.Url = mediaItem.URL;
                media.UpdatedAt = DateTime.UtcNow;

                // Update the media in the database
                await _unitOfWork.Media.UpdateAsync(media);
            }
            await _unitOfWork.CompleteAsync();

            return true;
        }

        public async Task<bool> DeleteHotelMediaAsync(int mediaId)
        {
            var media = await _unitOfWork.Media.GetByIdAsync(mediaId);
            if (media == null) return false;

            await _unitOfWork.Media.DeleteAsync(media);
            await _unitOfWork.CompleteAsync();

            return true;
        }
    }
}
