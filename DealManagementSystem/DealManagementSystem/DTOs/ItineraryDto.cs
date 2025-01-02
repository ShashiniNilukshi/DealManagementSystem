namespace DealManagementSystem.DTOs
{
    public class ItineraryDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } // Required
        public int Day { get; set; } // Day in the itinerary sequence (1, 2, 3, ...)
    }
}
