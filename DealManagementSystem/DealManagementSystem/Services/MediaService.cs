using DealManagementSystem.DTOs;
using DealManagementSystem.Interfaces;
using DealManagementSystem.Models;
using DealManagementSystem.Models.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DealManagementSystem.Services
{
    public class MediaService : IMediaService
    {
        private readonly IUnitOfWork _unitOfWork;

        public MediaService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public async Task<List<MediaDTO>> GetMediaByHotelIdAsync(int hotelId)
        {
            if (hotelId <= 0)
            {
                throw new ArgumentException("Hotel ID must be positive", nameof(hotelId));
            }

            var hotel = await _unitOfWork.Hotels
                .Include(h => h.Media) // Eagerly load the related Media collection
                .FirstOrDefaultAsync(h => h.Id == hotelId); // Ensure the hotel is found

            if (hotel == null)
            {
                return new List<MediaDTO>(); // Return an empty list if the hotel is not found
            }

            return hotel.Media
                .Select(m => new MediaDTO
                {
                    Type = m.Type.ToString(),  // Convert enum to string
                    URL = m.Url
                })
                .ToList();
        }

        public async Task<MediaDTO> AddHotelMediaAsync(int hotelId, AddHotelMediaDTO mediaDto)
        {
            if (hotelId <= 0)
            {
                throw new ArgumentException("Hotel ID must be positive", nameof(hotelId));
            }

            if (mediaDto == null)
            {
                throw new ArgumentNullException(nameof(mediaDto));
            }

            var hotel = await _unitOfWork.Hotels
                .Include(h => h.Media) // Eagerly load the related Media collection
                .FirstOrDefaultAsync(h => h.Id == hotelId); // Ensure the hotel is found

            if (hotel == null)
            {
                throw new KeyNotFoundException($"Hotel with ID {hotelId} not found");
            }

            // Ensure the Media collection is initialized
            if (hotel.Media == null)
            {
                hotel.Media = new List<Media>();
            }

            ValidateMediaUrl(mediaDto.Media.URL);

            // Parse the string to MediaType enum
            if (!Enum.TryParse<MediaType>(mediaDto.Media.Type, true, out MediaType mediaType))
            {
                throw new ArgumentException($"Invalid media type: {mediaDto.Media.Type}");
            }

            var media = new Media
            {
                //Type = mediaType, // Uncomment if you want to store the media type
                Url = mediaDto.Media.URL,
                CreatedAt = DateTime.UtcNow,
                HotelId = hotelId
            };

            hotel.Media.Add(media); // Add new media to the hotel's collection
            await _unitOfWork.Hotels.UpdateAsync(hotel);
            await _unitOfWork.CompleteAsync();

            return new MediaDTO
            {
                Type = media.Type.ToString(), // Convert enum to string
                URL = media.Url
            };
        }

        public async Task<MediaDTO> UpdateHotelMediaAsync(int mediaId, UpdateHotelMediaDTO mediaDto)
        {
            if (mediaId <= 0)
            {
                throw new ArgumentException("Media ID must be positive", nameof(mediaId));
            }

            if (mediaDto == null || !mediaDto.Media.Any())
            {
                throw new ArgumentException("Media update data is required", nameof(mediaDto));
            }

            var media = await _unitOfWork.Media
                .Include(m => m.Hotel) // Eagerly load related Hotel if needed
                .FirstOrDefaultAsync(m => m.Id == mediaId); // Ensure media is found

            if (media == null)
            {
                throw new KeyNotFoundException($"Media with ID {mediaId} not found");
            }

            var mediaToUpdate = mediaDto.Media.FirstOrDefault(m => m.URL == media.Url);
            if (mediaToUpdate == null)
            {
                throw new InvalidOperationException("No matching media found to update");
            }

            ValidateMediaUrl(mediaToUpdate.URL);

            // Parse the string to MediaType enum
            if (!Enum.TryParse<MediaType>(mediaToUpdate.Type, true, out MediaType mediaType))
            {
                throw new ArgumentException($"Invalid media type: {mediaToUpdate.Type}");
            }

            // Update media details
            media.Url = mediaToUpdate.URL;
            media.UpdatedAt = DateTime.UtcNow;

            // Ensure that the Hotel and other related entities are still valid if required
            await _unitOfWork.Media.UpdateAsync(media);
            await _unitOfWork.CompleteAsync();

            return new MediaDTO
            {
                Type = media.Type.ToString(),  // Convert enum to string
                URL = media.Url
            };
        }

        public async Task<bool> DeleteHotelMediaAsync(int mediaId)
        {
            if (mediaId <= 0)
            {
                throw new ArgumentException("Media ID must be positive", nameof(mediaId));
            }

            var media = await _unitOfWork.Media.GetByIdAsync(mediaId);
            if (media == null)
            {
                return false;
            }

            await _unitOfWork.Media.DeleteAsync(media);
            await _unitOfWork.CompleteAsync();
            return true;
        }

        // Implement the missing methods
        public async Task<List<MediaDTO>> GetMediaByDealIdAsync(int dealId)
        {
            // Implementation for fetching media by deal ID
            // This is a placeholder implementation, replace with actual logic
            return new List<MediaDTO>();
        }

        public async Task<MediaDTO> AddMediaToDealAsync(int dealId, AddMediaToDealDTO mediaDto)
        {
            // Implementation for adding media to a deal
            // This is a placeholder implementation, replace with actual logic
            return new MediaDTO();
        }

        public async Task<MediaDTO> UpdateMediaAsync(int mediaId, MediaDTO mediaDto)
        {
            // Implementation for updating media
            // This is a placeholder implementation, replace with actual logic
            return new MediaDTO();
        }

        public async Task<bool> DeleteMediaAsync(int mediaId)
        {
            // Implementation for deleting media
            // This is a placeholder implementation, replace with actual logic
            return true;
        }

        private static void ValidateMediaUrl(string url)
        {
            if (string.IsNullOrWhiteSpace(url))
            {
                throw new ArgumentException("URL cannot be empty", nameof(url));
            }

            if (!Uri.TryCreate(url, UriKind.Absolute, out _))
            {
                throw new ArgumentException("Invalid URL format", nameof(url));
            }
        }
    }
}
