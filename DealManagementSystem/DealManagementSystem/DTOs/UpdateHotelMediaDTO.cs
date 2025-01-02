namespace DealManagementSystem.DTOs
{
    public class UpdateHotelMediaDTO
    {
        public int HotelId { get; set; } // The ID of the hotel
        public List<MediaDTO> Media { get; set; } // List of media (images/videos)
      
    }

}
