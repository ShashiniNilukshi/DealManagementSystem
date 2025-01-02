// Models/User.cs
using DealManagementSystem.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace DealManagementSystem.Models
{
    public class User : BaseEntity
    {
        public int Id { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string PasswordHash { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        public UserRole Role { get; set; } = UserRole.User; // Default role

        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}