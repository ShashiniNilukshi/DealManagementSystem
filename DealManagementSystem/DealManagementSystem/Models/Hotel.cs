namespace DealManagementSystem.Models
{
    public class Hotel : BaseEntity
    {
        public string Name { get; set; }
        public decimal Rate { get; set; }
        public string Amenities { get; set; }
        public List<Media> Media { get; set; }
        public Deals Deal { get; set; } 
        public int DealId { get; set; }
    }
}