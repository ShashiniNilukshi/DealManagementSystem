namespace DealManagementSystem.DTOs
{
    public class AddHotelToDealDTO
    {
        public int DealId { get; set; } // The ID of the deal
        public HotelDTO Hotel { get; set; } // The hotel to add to the deal
    }

}
