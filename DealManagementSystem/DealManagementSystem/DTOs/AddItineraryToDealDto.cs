namespace DealManagementSystem.DTOs
{
    public class AddItineraryToDealDTO
    {
        public int DealId { get; set; } // The ID of the deal
        public ItineraryDTO Itinerary { get; set; } // The itinerary to add to the deal
    }

}
