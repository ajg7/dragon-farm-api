using Microsoft.EntityFrameworkCore;
using DragonFarmApi.Models;

namespace DragonFarmApi.Data
{
    public class DragonFarmContext : DbContext
    {
        public DragonFarmContext(DbContextOptions<DragonFarmContext> options) : base(options)
        {
        }

        public DbSet<Dragon> Dragons { get; set; }
        public DbSet<FeedingRecord> FeedingRecords { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure Dragon entity
            modelBuilder.Entity<Dragon>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Species).IsRequired().HasMaxLength(50);
                entity.Property(e => e.Color).IsRequired().HasMaxLength(30);
                entity.Property(e => e.Description).HasMaxLength(500);
            });

            // Configure FeedingRecord entity
            modelBuilder.Entity<FeedingRecord>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.FoodType).IsRequired().HasMaxLength(50);
                entity.Property(e => e.Notes).HasMaxLength(200);
                
                // Configure relationship
                entity.HasOne(e => e.Dragon)
                      .WithMany(d => d.FeedingRecords)
                      .HasForeignKey(e => e.DragonId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // Seed data
            modelBuilder.Entity<Dragon>().HasData(
                new Dragon 
                { 
                    Id = 1, 
                    Name = "Smaug", 
                    Species = "Fire Dragon", 
                    Color = "Red", 
                    Age = 150, 
                    Weight = 2500.5,
                    Description = "A fierce fire-breathing dragon",
                    DateAcquired = new DateTime(2020, 1, 15)
                },
                new Dragon 
                { 
                    Id = 2, 
                    Name = "Frostbite", 
                    Species = "Ice Dragon", 
                    Color = "Blue", 
                    Age = 75, 
                    Weight = 1800.0,
                    Description = "A majestic ice dragon from the northern mountains",
                    DateAcquired = new DateTime(2021, 6, 10)
                }
            );
        }
    }
}