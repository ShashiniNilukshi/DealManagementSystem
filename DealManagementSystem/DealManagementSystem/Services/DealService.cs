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
    public class DealService : IDealService
    {
        private readonly IUnitOfWork _unitOfWork;

        public DealService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<DealDTO>> GetAllDealsAsync()
        {
            var deals = await _unitOfWork.Deals
                .Include(d => d.Hotels)
                    .ThenInclude(h => h.Media) // Include media for hotels
                .Include(d => d.Itineraries) // Include itineraries
                .ToListAsync();

            return deals.Select(deal => new DealDTO
            {
                Id = deal.Id, // Include the Deal ID
                Name = deal.Name,
                Video = deal.VideoUrl, // Corrected property name
                Hotels = deal.Hotels.Select(h => new HotelDTO
                {
                    Id = h.Id, // Include the Hotel ID
                    Name = h.Name,
                    Rate = h.Rate,
                    Amenities = h.Amenities,
                    Media = h.Media.Select(m => new MediaDTO
                    {
                        Id = m.Id, // Include the Media ID
                        Type = m.Type.ToString(), // Convert MediaType to string
                        URL = m.Url
                    }).ToList()
                }).ToList(),
                Itineraries = deal.Itineraries.Select(i => new ItineraryDTO
                {
                    Id = i.Id, // Include the Itinerary ID
                    Name = i.Name,
                    Day = i.Day
                }).ToList()
            }).ToList();
        }


        public async Task<DealDTO> GetDealByIdAsync(int id)
        {
            var deal = await _unitOfWork.Deals
                .Include(d => d.Hotels)
                    .ThenInclude(h => h.Media) // Include media for hotels
                .Include(d => d.Itineraries) // Include itineraries
                .FirstOrDefaultAsync(d => d.Id == id);

            return deal == null ? null : new DealDTO
            {
                Id = deal.Id, // Include the Deal ID
                Name = deal.Name,
                Video = deal.VideoUrl, // Corrected property name
                Hotels = deal.Hotels.Select(h => new HotelDTO
                {
                    Id = h.Id, // Include the Hotel ID
                    Name = h.Name,
                    Rate = h.Rate,
                    Amenities = h.Amenities,
                    Media = h.Media.Select(m => new MediaDTO
                    {
                        Id = m.Id, // Include the Media ID
                        Type = m.Type.ToString(), // Convert MediaType to string
                        URL = m.Url
                    }).ToList()
                }).ToList(),
                Itineraries = deal.Itineraries.Select(i => new ItineraryDTO
                {
                    Id = i.Id, // Include the Itinerary ID
                    Name = i.Name,
                    Day = i.Day
                }).ToList()
            };
        }


        public async Task<DealDTO> CreateDealAsync(DealDTO dealDto)
        {
            var slug = await GenerateSlugAsync(dealDto.Name);

            var deal = new Deals
            {
                Name = dealDto.Name,
                Slug = slug,
                VideoUrl = dealDto.Video,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = null,
                Hotels = new List<Hotel>(),
                Itineraries = new List<Itinerary>()
            };

            // Add Hotels
            if (dealDto.Hotels != null)
            {
                foreach (var hotelDto in dealDto.Hotels)
                {
                    var hotel = new Hotel
                    {
                        Name = hotelDto.Name,
                        Rate = hotelDto.Rate,
                        Amenities = hotelDto.Amenities,
                        CreatedAt = DateTime.UtcNow,
                        Media = new List<Media>()
                    };

                    // Add Media for the hotel
                    if (hotelDto.Media != null)
                    {
                        foreach (var mediaDto in hotelDto.Media)
                        {
                            hotel.Media.Add(new Media
                            {
                                Type = Enum.Parse<MediaType>(mediaDto.Type), // Convert string to enum
                                Url = mediaDto.URL,
                                CreatedAt = DateTime.UtcNow
                            });
                        }
                    }

                    deal.Hotels.Add(hotel);
                }
            }

            // Add Itineraries
            if (dealDto.Itineraries != null)
            {
                foreach (var itineraryDto in dealDto.Itineraries)
                {
                    deal.Itineraries.Add(new Itinerary
                    {
                        Name = itineraryDto.Name,
                        Day = itineraryDto.Day,
                        CreatedAt = DateTime.UtcNow
                    });
                }
            }

            await _unitOfWork.Deals.AddAsync(deal);
            await _unitOfWork.CompleteAsync();

            return MapToDealDto(deal); // Return the newly created deal, with auto-generated IDs
        }

        public async Task<DealDTO> UpdateDealAsync(int id, DealDTO dealDto)
        {
            var deal = await _unitOfWork.Deals
                .Include(d => d.Hotels)
                    .ThenInclude(h => h.Media)
                .Include(d => d.Itineraries)
                .FirstOrDefaultAsync(d => d.Id == id);

            if (deal == null) return null;

            // Initialize collections if null
            deal.Hotels ??= new List<Hotel>();
            deal.Itineraries ??= new List<Itinerary>();
            bool isUpdated = false;

            if (!string.IsNullOrEmpty(dealDto.Name) && dealDto.Name != deal.Name)
            {
                deal.Name = dealDto.Name;
                deal.Slug = await GenerateSlugAsync(dealDto.Name);
                isUpdated = true;
            }

            if (!string.IsNullOrEmpty(dealDto.Video) && dealDto.Video != deal.VideoUrl)
            {
                deal.VideoUrl = dealDto.Video;
                isUpdated = true;
            }

            if (dealDto.Itineraries?.Any() == true)
            {
                foreach (var itineraryDto in dealDto.Itineraries)
                {
                    var existingItinerary = deal.Itineraries
                        .FirstOrDefault(i => i.Name == itineraryDto.Name);

                    if (existingItinerary != null)
                    {
                        if (itineraryDto.Day > 0 && itineraryDto.Day != existingItinerary.Day)
                        {
                            existingItinerary.Day = itineraryDto.Day;
                            isUpdated = true;
                        }
                    }
                    else
                    {
                        deal.Itineraries.Add(new Itinerary
                        {
                            Name = itineraryDto.Name,
                            Day = itineraryDto.Day,
                            CreatedAt = DateTime.UtcNow
                        });
                        isUpdated = true;
                    }
                }
            }

            if (dealDto.Hotels?.Any() == true)
            {
                foreach (var hotelDto in dealDto.Hotels)
                {
                    var existingHotel = deal.Hotels
                        .FirstOrDefault(h => h.Name == hotelDto.Name);

                    if (existingHotel != null)
                    {
                        if (hotelDto.Rate > 0 && hotelDto.Rate != existingHotel.Rate)
                        {
                            existingHotel.Rate = hotelDto.Rate;
                            isUpdated = true;
                        }

                        if (!string.IsNullOrEmpty(hotelDto.Amenities) &&
                            hotelDto.Amenities != existingHotel.Amenities)
                        {
                            existingHotel.Amenities = hotelDto.Amenities;
                            isUpdated = true;
                        }

                        if (hotelDto.Media?.Any() == true)
                        {
                            existingHotel.Media ??= new List<Media>();

                            foreach (var mediaDto in hotelDto.Media)
                            {
                                var existingMedia = existingHotel.Media
                                    .FirstOrDefault(m => m.Url == mediaDto.URL);

                                if (existingMedia != null)
                                {
                                    if (existingMedia.Type != Enum.Parse<MediaType>(mediaDto.Type) ||
                                        existingMedia.Url != mediaDto.URL)
                                    {
                                        existingMedia.Type = Enum.Parse<MediaType>(mediaDto.Type);
                                        existingMedia.Url = mediaDto.URL;
                                        isUpdated = true;
                                    }
                                }
                                else
                                {
                                    existingHotel.Media.Add(new Media
                                    {
                                        Type = Enum.Parse<MediaType>(mediaDto.Type),
                                        Url = mediaDto.URL,
                                        CreatedAt = DateTime.UtcNow
                                    });
                                    isUpdated = true;
                                }
                            }
                        }
                    }
                    else
                    {
                        var newHotel = new Hotel
                        {
                            Name = hotelDto.Name,
                            Rate = hotelDto.Rate,
                            Amenities = hotelDto.Amenities,
                            CreatedAt = DateTime.UtcNow,
                            Media = new List<Media>()
                        };

                        if (hotelDto.Media?.Any() == true)
                        {
                            foreach (var mediaDto in hotelDto.Media)
                            {
                                newHotel.Media.Add(new Media
                                {
                                    Type = Enum.Parse<MediaType>(mediaDto.Type),
                                    Url = mediaDto.URL,
                                    CreatedAt = DateTime.UtcNow
                                });
                            }
                        }

                        deal.Hotels.Add(newHotel);
                        isUpdated = true;
                    }
                }
            }

            if (isUpdated)
            {
                deal.UpdatedAt = DateTime.UtcNow;
                await _unitOfWork.Deals.UpdateAsync(deal);
                await _unitOfWork.CompleteAsync();
            }

            return MapToDealDto(deal);
        }

        public async Task<bool> DeleteDealAsync(int id)
        {
            var deal = await _unitOfWork.Deals.GetByIdAsync(id);
            if (deal == null) return false;

            await _unitOfWork.Deals.DeleteAsync(deal);
            await _unitOfWork.CompleteAsync();

            return true;
        }

        private async Task<string> GenerateSlugAsync(string name)
        {
            var slug = name.ToLower()
                .Replace(" ", "-")
                .Replace("_", "-")
                .Replace(".", "")
                .Replace(",", "")
                .Replace("!", "")
                .Replace("?", "");

            var existingDeals = await _unitOfWork.Deals.GetAllAsync();
            var isUnique = !existingDeals.Any(d => d.Slug == slug);

            if (!isUnique)
            {
                slug = $"{slug}-{Guid.NewGuid().ToString().Substring(0, 8)}";
            }

            return slug;
        }

        private static DealDTO MapToDealDto(Deals deal)
        {
            return new DealDTO
            {
                Id = deal.Id,
                Name = deal.Name,
                Slug = deal.Slug,
                Video = deal.VideoUrl,
                CreatedAt = deal.CreatedAt,
                UpdatedAt = deal.UpdatedAt ?? DateTime.MinValue,
                Hotels = deal.Hotels.Select(h => new HotelDTO
                {
                    Name = h.Name,
                    Rate = h.Rate,
                    Amenities = h.Amenities,
                    Media = h.Media.Select(m => new MediaDTO
                    {
                        Type = m.Type.ToString(),
                        URL = m.Url
                    }).ToList()
                }).ToList(),
                Itineraries = deal.Itineraries.Select(i => new ItineraryDTO
                {
                    Name = i.Name,
                    Day = i.Day
                }).ToList()
            };
        }
    }
}