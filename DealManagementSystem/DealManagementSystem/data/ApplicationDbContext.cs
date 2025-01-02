using Microsoft.EntityFrameworkCore;
using DealManagementSystem.Models;

namespace DealManagementSystem.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Deals> Deals { get; set; }
        public DbSet<Hotel> Hotels { get; set; }
        public DbSet<Itinerary> Itineraries { get; set; }
        public DbSet<Media> Media { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure Deal entity
            modelBuilder.Entity<Deals>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Slug).IsRequired().HasMaxLength(200);
                entity.HasIndex(e => e.Slug).IsUnique(); // Ensure Slug is unique

                // Relationship: Deal -> Hotels (one-to-many)
                entity.HasMany(d => d.Hotels)
                    .WithOne(h => h.Deal)
                    .HasForeignKey(h => h.DealId)
                    .OnDelete(DeleteBehavior.Cascade); // Cascade delete hotels when deal is deleted

                // Relationship: Deal -> Itineraries (one-to-many)
                entity.HasMany(d => d.Itineraries)
                    .WithOne(i => i.Deal)
                    .HasForeignKey(i => i.DealId)
                    .OnDelete(DeleteBehavior.Cascade); // Cascade delete itineraries when deal is deleted
            });

            // Configure Hotel entity
            modelBuilder.Entity<Hotel>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Rate).HasPrecision(3, 1); // Precision for the Rate field
                entity.Property(e => e.Amenities).HasMaxLength(500);

                // Relationship: Hotel -> Media (one-to-many)
                entity.HasMany(h => h.Media)
                    .WithOne(m => m.Hotel)
                    .HasForeignKey(m => m.HotelId)
                    .OnDelete(DeleteBehavior.Cascade); // Cascade delete media when hotel is deleted
            });

            // Configure Itinerary entity
            modelBuilder.Entity<Itinerary>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Day).IsRequired(); // Day is required for each itinerary
            });

            // Configure Media entity
            modelBuilder.Entity<Media>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Url).IsRequired().HasMaxLength(500);
                entity.Property(e => e.Type).IsRequired(); // Type is required for each media
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Email).IsRequired().HasMaxLength(100);
                entity.HasIndex(e => e.Email).IsUnique();
                entity.Property(e => e.PasswordHash).IsRequired();
                entity.Property(e => e.Role).HasConversion<string>();
            });
        }
    }
}
