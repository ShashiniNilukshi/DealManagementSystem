namespace DealManagementSystem.Models
{
    public class BaseEntity
    {
        public int Id { get; set; }  // Primary key for every entity
        public DateTime CreatedAt { get; set; }  // When the entity was created
        public DateTime? UpdatedAt { get; set; }  // When the entity was last updated (nullable)
        public bool IsDeleted { get; set; }  // Soft delete flag to mark records as deleted without actually removing them
    }
}
