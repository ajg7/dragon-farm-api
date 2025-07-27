using DragonFarmApi;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DragonFarmApi
{
    public class DbInitializer
    {
        public static async Task Main(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var services = new ServiceCollection();
            services.AddDbContext<DragonFarmContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            var serviceProvider = services.BuildServiceProvider();

            using var scope = serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<DragonFarmContext>();

            try 
            {
                Console.WriteLine("Creating and seeding database...");
                await context.Database.EnsureCreatedAsync();
                Console.WriteLine("Database created and seeded successfully!");
                
                // Verify the dragons are created
                var dragons = await context.Dragons.ToListAsync();
                Console.WriteLine($"Dragons in database: {dragons.Count}");
                foreach (var dragon in dragons)
                {
                    Console.WriteLine($"- {dragon.Name} ({dragon.Sex})");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating database: {ex.Message}");
            }
        }
    }
}