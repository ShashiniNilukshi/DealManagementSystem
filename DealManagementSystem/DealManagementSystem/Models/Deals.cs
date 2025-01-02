namespace DealManagementSystem.Models
{
    public class Deals : BaseEntity
    {
        public string Name { get; set; }
        public string Slug { get; set; }
        public string VideoUrl { get; set; }
        public List<Hotel> Hotels { get; set; }
        public List<Itinerary> Itineraries { get; set; }
    }
}
