namespace DealManagementSystem.DTOs
{
    public class DealDTO
    {
        public int Id { get; set; } // Unique identifier
        public string Name { get; set; } // Required
        public string Slug { get; set; } // Unique identifier
        public string Video { get; set; } // URL to a video resource
        public List<HotelDTO> Hotels { get; set; } // List of hotels in the deal
        public List<ItineraryDTO> Itineraries { get; set; } // List of itineraries in the deal
        public DateTime CreatedAt { get; set; } // Date the deal was created
        public DateTime UpdatedAt { get; set; } // Date the deal was last updated
    }

}
