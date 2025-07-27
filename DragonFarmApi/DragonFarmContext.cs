using DragonFarmApi.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DragonFarmApi;
public class DragonFarmContext : IdentityDbContext<ApplicationUser, ApplicationRole, string>
{
    public DragonFarmContext(DbContextOptions<DragonFarmContext> options)
        : base(options) { }

    public virtual DbSet<Dragon> Dragons { get; set; }
    public virtual DbSet<DragonTrait> DragonTraits { get; set; }
    public virtual DbSet<Trait> Traits { get; set; }

    protected override void OnModelCreating(ModelBuilder b)
    {
        base.OnModelCreating(b);

        // Configure DragonTrait composite key
        b.Entity<DragonTrait>()
            .HasKey(x => new { x.DragonId, x.TraitId });

        // Configure relationships
        b.Entity<DragonTrait>()
            .HasOne(dt => dt.Dragon)
            .WithMany(d => d.Traits)
            .HasForeignKey(dt => dt.DragonId);

        b.Entity<DragonTrait>()
            .HasOne(dt => dt.Trait)
            .WithMany()
            .HasForeignKey(dt => dt.TraitId);

        // Configure char properties explicitly
        b.Entity<Trait>()
            .Property(t => t.DominantAllele)
            .HasColumnType("nvarchar(1)");

        b.Entity<Trait>()
            .Property(t => t.RecessiveAllele)
            .HasColumnType("nvarchar(1)");

        b.Entity<DragonTrait>()
            .Property(dt => dt.AlleleA)
            .HasColumnType("nvarchar(1)");

        b.Entity<DragonTrait>()
            .Property(dt => dt.AlleleB)
            .HasColumnType("nvarchar(1)");

        // Seed roles
        var adminRoleId = "1";
        var userRoleId = "2";
        var managerRoleId = "3";

        b.Entity<ApplicationRole>().HasData(
            new ApplicationRole 
            { 
                Id = adminRoleId, 
                Name = "Admin", 
                NormalizedName = "ADMIN", 
                Description = "Full access to all dragon farm operations",
                CreatedAt = new DateTimeOffset(2025, 1, 1, 0, 0, 0, TimeSpan.Zero)
            },
            new ApplicationRole 
            { 
                Id = userRoleId, 
                Name = "User", 
                NormalizedName = "USER", 
                Description = "Basic access to view dragons and traits",
                CreatedAt = new DateTimeOffset(2025, 1, 1, 0, 0, 0, TimeSpan.Zero)
            },
            new ApplicationRole 
            { 
                Id = managerRoleId, 
                Name = "Manager", 
                NormalizedName = "MANAGER", 
                Description = "Can manage dragons and breeding operations",
                CreatedAt = new DateTimeOffset(2025, 1, 1, 0, 0, 0, TimeSpan.Zero)
            });

        // Seed trait data
        b.Entity<Trait>().HasData(
            new Trait { Id = 1, Name = "Color", DominantAllele = 'R', RecessiveAllele = 'r' },
            new Trait { Id = 2, Name = "WingSpan", DominantAllele = 'W', RecessiveAllele = 'w' },
            new Trait { Id = 3, Name = "Claw", DominantAllele = 'S', RecessiveAllele = 's' });

        // Seed dragon data with fixed GUIDs
        var starterAId = new Guid("11111111-1111-1111-1111-111111111111");
        var starterBId = new Guid("22222222-2222-2222-2222-222222222222");
        var fixedDate = new DateTimeOffset(2025, 1, 1, 0, 0, 0, TimeSpan.Zero);

        b.Entity<Dragon>().HasData(
            new Dragon { Id = starterAId, Name = "Pyro", Sex = DragonSex.Male, HatchedAt = fixedDate },
            new Dragon { Id = starterBId, Name = "Astra", Sex = DragonSex.Female, HatchedAt = fixedDate });
    }
}
