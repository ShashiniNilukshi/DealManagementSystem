namespace DealManagementSystem.Models
{
    public class Itinerary : BaseEntity
    {
        public string Name { get; set; }
        public int Day { get; set; }
        public Deals Deal { get; set; } // Updated property name
        public int DealId { get; set; }
    }
}