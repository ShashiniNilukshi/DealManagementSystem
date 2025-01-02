using DealManagementSystem.Models;

namespace DealManagementSystem.DTOs
{
    public class HotelDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } // Required
        public decimal Rate { get; set; } // Rate range 1.0 - 5.0
        public string Amenities { get; set; } // Comma-separated values (e.g., "Wi-Fi, Pool, Gym")
        public List<MediaDTO> Media { get; set; } // Change Media to MediaDTO
    }
}