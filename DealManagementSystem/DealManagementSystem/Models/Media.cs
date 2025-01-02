using DealManagementSystem.Models.Enums;

namespace DealManagementSystem.Models
{
    public class Media : BaseEntity
    {
        public MediaType Type { get; set; }
        public string Url { get; set; }
        public Hotel Hotel { get; set; }
        public int HotelId { get; set; }
    }
}