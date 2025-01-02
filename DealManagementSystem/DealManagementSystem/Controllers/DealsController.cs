using DealManagementSystem.DTOs;
using DealManagementSystem.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DealManagementSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] // Base authorization for all endpoints
    public class DealsController : ControllerBase
    {
        private readonly IDealService _dealService;
        private readonly IHotelService _hotelService;
        private readonly IItineraryService _itineraryService;
        private readonly IMediaService _mediaService;
        private readonly ILogger<DealsController> _logger;

        public DealsController(
            IDealService dealService,
            IHotelService hotelService,
            IItineraryService itineraryService,
            IMediaService mediaService,
            ILogger<DealsController> logger)
        {
            _dealService = dealService;
            _hotelService = hotelService;
            _itineraryService = itineraryService;
            _mediaService = mediaService;
            _logger = logger;
        }

        // CRUD for Deals
        [HttpGet]
        [Authorize(Roles = "User,Admin,SuperAdmin")]
        public async Task<ActionResult<List<DealDTO>>> GetDeals()
        {
            _logger.LogInformation("Getting all deals...");
            try
            {
                var deals = await _dealService.GetAllDealsAsync();
                _logger.LogInformation("Successfully fetched {DealCount} deals.", deals.Count);
                return Ok(deals);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching deals.");
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "User,Admin,SuperAdmin")]
        public async Task<ActionResult<DealDTO>> GetDeal(int id)
        {
            _logger.LogInformation("Fetching deal with ID {DealId}", id);
            try
            {
                var deal = await _dealService.GetDealByIdAsync(id);
                if (deal == null)
                {
                    _logger.LogWarning("Deal with ID {DealId} not found.", id);
                    return NotFound();
                }
                _logger.LogInformation("Successfully fetched deal with ID {DealId}.", id);
                return Ok(deal);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching deal with ID {DealId}.", id);
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin,SuperAdmin")]
        public async Task<ActionResult<DealDTO>> CreateDeal(DealDTO dealDto)
        {
            _logger.LogInformation("Creating a new deal with Name: {DealName}", dealDto.Name);
            try
            {
                var deal = await _dealService.CreateDealAsync(dealDto);
                _logger.LogInformation("Successfully created deal with ID {DealId}.", deal.Id);
                return CreatedAtAction(nameof(GetDeal), new { id = deal.Id }, deal);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating deal with Name: {DealName}", dealDto.Name);
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,SuperAdmin")]
        public async Task<ActionResult<DealDTO>> UpdateDeal(int id, DealDTO dealDto)
        {
            _logger.LogInformation("Updating deal with ID {DealId}.", id);
            try
            {
                var deal = await _dealService.UpdateDealAsync(id, dealDto);
                if (deal == null)
                {
                    _logger.LogWarning("Deal with ID {DealId} not found for update.", id);
                    return NotFound();
                }
                _logger.LogInformation("Successfully updated deal with ID {DealId}.", id);
                return Ok(deal);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating deal with ID {DealId}.", id);
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> DeleteDeal(int id)
        {
            _logger.LogInformation("Deleting deal with ID {DealId}.", id);
            try
            {
                var result = await _dealService.DeleteDealAsync(id);
                if (!result)
                {
                    _logger.LogWarning("Deal with ID {DealId} not found for deletion.", id);
                    return NotFound();
                }
                _logger.LogInformation("Successfully deleted deal with ID {DealId}.", id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting deal with ID {DealId}.", id);
                return StatusCode(500, "Internal Server Error");
            }
        }

        // CRUD for Hotels
        [HttpGet("{dealId}/hotels")]
        [Authorize(Roles = "User,Admin,SuperAdmin")]
        public async Task<ActionResult<List<HotelDTO>>> GetHotelsByDeal(int dealId)
        {
            _logger.LogInformation("Fetching hotels for deal with ID {DealId}.", dealId);
            try
            {
                var hotels = await _hotelService.GetHotelsByDealIdAsync(dealId);
                if (hotels == null)
                {
                    _logger.LogWarning("No hotels found for deal with ID {DealId}.", dealId);
                    return NotFound();
                }
                _logger.LogInformation("Successfully fetched {HotelCount} hotels for deal with ID {DealId}.", hotels.Count, dealId);
                return Ok(hotels);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching hotels for deal with ID {DealId}.", dealId);
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpPost("{dealId}/hotels")]
        [Authorize(Roles = "Admin,SuperAdmin")]
        public async Task<ActionResult<HotelDTO>> AddHotelToDeal(int dealId, HotelDTO hotelDto)
        {
            _logger.LogInformation("Adding hotel to deal with ID {DealId} and Hotel Name {HotelName}.", dealId, hotelDto.Name);
            try
            {
                var hotel = await _hotelService.AddHotelToDealAsync(dealId, hotelDto);
                if (hotel == null)
                {
                    _logger.LogWarning("Failed to add hotel to deal with ID {DealId}.", dealId);
                    return NotFound();
                }
                _logger.LogInformation("Successfully added hotel to deal with ID {DealId}.", dealId);
                return CreatedAtAction(nameof(GetHotelsByDeal), new { dealId }, hotel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while adding hotel to deal with ID {DealId}.", dealId);
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpPut("{dealId}/hotels/{hotelId}")]
        [Authorize(Roles = "Admin,SuperAdmin")]
        public async Task<ActionResult<HotelDTO>> UpdateHotel(int dealId, int hotelId, HotelDTO hotelDto)
        {
            _logger.LogInformation("Updating hotel with ID {HotelId} for deal with ID {DealId}.", hotelId, dealId);
            try
            {
                var hotel = await _hotelService.UpdateHotelAsync(hotelId, hotelDto);
                if (hotel == null)
                {
                    _logger.LogWarning("Hotel with ID {HotelId} not found for update in deal with ID {DealId}.", hotelId, dealId);
                    return NotFound();
                }
                _logger.LogInformation("Successfully updated hotel with ID {HotelId} for deal with ID {DealId}.", hotelId, dealId);
                return Ok(hotel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating hotel with ID {HotelId} for deal with ID {DealId}.", hotelId, dealId);
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpDelete("{dealId}/hotels/{hotelId}")]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> DeleteHotel(int dealId, int hotelId)
        {
            _logger.LogInformation("Deleting hotel with ID {HotelId} from deal with ID {DealId}.", hotelId, dealId);
            try
            {
                var result = await _hotelService.DeleteHotelAsync(hotelId);
                if (!result)
                {
                    _logger.LogWarning("Hotel with ID {HotelId} not found for deletion in deal with ID {DealId}.", hotelId, dealId);
                    return NotFound();
                }
                _logger.LogInformation("Successfully deleted hotel with ID {HotelId} from deal with ID {DealId}.", hotelId, dealId);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting hotel with ID {HotelId} from deal with ID {DealId}.", hotelId, dealId);
                return StatusCode(500, "Internal Server Error");
            }
        }

        // CRUD for Itineraries
        [HttpGet("{dealId}/itineraries")]
        [Authorize(Roles = "User,Admin,SuperAdmin")]
        public async Task<ActionResult<List<ItineraryDTO>>> GetItinerariesByDeal(int dealId)
        {
            _logger.LogInformation("Fetching itineraries for deal with ID {DealId}.", dealId);
            try
            {
                var itineraries = await _itineraryService.GetItinerariesByDealIdAsync(dealId);
                if (itineraries == null)
                {
                    _logger.LogWarning("No itineraries found for deal with ID {DealId}.", dealId);
                    return NotFound();
                }
                _logger.LogInformation("Successfully fetched {ItineraryCount} itineraries for deal with ID {DealId}.", itineraries.Count, dealId);
                return Ok(itineraries);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching itineraries for deal with ID {DealId}.", dealId);
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpPost("{dealId}/itineraries")]
        [Authorize(Roles = "Admin,SuperAdmin")]
        public async Task<ActionResult<ItineraryDTO>> AddItineraryToDeal(int dealId, AddItineraryToDealDTO itineraryDto)
        {
            _logger.LogInformation("Adding itinerary to deal with ID {DealId}.", dealId);
            try
            {
                var itinerary = await _itineraryService.AddItineraryToDealAsync(dealId, itineraryDto);
                if (itinerary == null)
                {
                    _logger.LogWarning("Failed to add itinerary to deal with ID {DealId}.", dealId);
                    return NotFound();
                }
                _logger.LogInformation("Successfully added itinerary to deal with ID {DealId}.", dealId);
                return CreatedAtAction(nameof(GetItinerariesByDeal), new { dealId }, itinerary);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while adding itinerary to deal with ID {DealId}.", dealId);
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpPut("{dealId}/itineraries/{itineraryId}")]
        [Authorize(Roles = "Admin,SuperAdmin")]
        public async Task<ActionResult<ItineraryDTO>> UpdateItinerary(int dealId, int itineraryId, ItineraryDTO itineraryDto)
        {
            _logger.LogInformation("Updating itinerary with ID {ItineraryId} for deal with ID {DealId}.", itineraryId, dealId);
            try
            {
                var itinerary = await _itineraryService.UpdateItineraryAsync(itineraryId, itineraryDto);
                if (itinerary == null)
                {
                    _logger.LogWarning("Itinerary with ID {ItineraryId} not found for update in deal with ID {DealId}.", itineraryId, dealId);
                    return NotFound();
                }
                _logger.LogInformation("Successfully updated itinerary with ID {ItineraryId} for deal with ID {DealId}.", itineraryId, dealId);
                return Ok(itinerary);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating itinerary with ID {ItineraryId} for deal with ID {DealId}.", itineraryId, dealId);
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpDelete("{dealId}/itineraries/{itineraryId}")]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> DeleteItinerary(int dealId, int itineraryId)
        {
            _logger.LogInformation("Deleting itinerary with ID {ItineraryId} from deal with ID {DealId}.", itineraryId, dealId);
            try
            {
                var result = await _itineraryService.DeleteItineraryAsync(itineraryId);
                if (!result)
                {
                    _logger.LogWarning("Itinerary with ID {ItineraryId} not found for deletion in deal with ID {DealId}.", itineraryId, dealId);
                    return NotFound();
                }
                _logger.LogInformation("Successfully deleted itinerary with ID {ItineraryId} from deal with ID {DealId}.", itineraryId, dealId);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting itinerary with ID {ItineraryId} from deal with ID {DealId}.", itineraryId, dealId);
                return StatusCode(500, "Internal Server Error");
            }
        }



        // CRUD for Media
        [HttpGet("{dealId}/media")]
        [Authorize(Roles = "User,Admin,SuperAdmin")]
        public async Task<ActionResult<List<MediaDTO>>> GetMediaByDeal(int dealId)
        {
            _logger.LogInformation("Fetching media for deal with ID {DealId}.", dealId);
            try
            {
                var mediaItems = await _mediaService.GetMediaByDealIdAsync(dealId);
                if (mediaItems == null)
                {
                    _logger.LogWarning("No media found for deal with ID {DealId}.", dealId);
                    return NotFound();
                }
                _logger.LogInformation("Successfully fetched {MediaCount} media items for deal with ID {DealId}.", mediaItems.Count, dealId);
                return Ok(mediaItems);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching media for deal with ID {DealId}.", dealId);
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpPost("{dealId}/media")]
        [Authorize(Roles = "Admin,SuperAdmin")]
        public async Task<ActionResult<MediaDTO>> AddMediaToDeal(int dealId, AddMediaToDealDTO mediaDto)
        {
            _logger.LogInformation("Adding media to deal with ID {DealId}.", dealId);
            try
            {
                var media = await _mediaService.AddMediaToDealAsync(dealId, mediaDto);
                if (media == null)
                {
                    _logger.LogWarning("Failed to add media to deal with ID {DealId}.", dealId);
                    return NotFound();
                }
                _logger.LogInformation("Successfully added media to deal with ID {DealId}.", dealId);
                return CreatedAtAction(nameof(GetMediaByDeal), new { dealId }, media);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while adding media to deal with ID {DealId}.", dealId);
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpPut("{dealId}/media/{mediaId}")]
        [Authorize(Roles = "Admin,SuperAdmin")]
        public async Task<ActionResult<MediaDTO>> UpdateMedia(int dealId, int mediaId, MediaDTO mediaDto)
        {
            _logger.LogInformation("Updating media with ID {MediaId} for deal with ID {DealId}.", mediaId, dealId);
            try
            {
                var media = await _mediaService.UpdateMediaAsync(mediaId, mediaDto);
                if (media == null)
                {
                    _logger.LogWarning("Media with ID {MediaId} not found for update in deal with ID {DealId}.", mediaId, dealId);
                    return NotFound();
                }
                _logger.LogInformation("Successfully updated media with ID {MediaId} for deal with ID {DealId}.", mediaId, dealId);
                return Ok(media);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating media with ID {MediaId} for deal with ID {DealId}.", mediaId, dealId);
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpDelete("{dealId}/media/{mediaId}")]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> DeleteMedia(int dealId, int mediaId)
        {
            _logger.LogInformation("Deleting media with ID {MediaId} from deal with ID {DealId}.", mediaId, dealId);
            try
            {
                var result = await _mediaService.DeleteMediaAsync(mediaId);
                if (!result)
                {
                    _logger.LogWarning("Media with ID {MediaId} not found for deletion in deal with ID {DealId}.", mediaId, dealId);
                    return NotFound();
                }
                _logger.LogInformation("Successfully deleted media with ID {MediaId} from deal with ID {DealId}.", mediaId, dealId);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting media with ID {MediaId} from deal with ID {DealId}.", mediaId, dealId);
                return StatusCode(500, "Internal Server Error");
            }
        }
    }
}
