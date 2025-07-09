using Microsoft.EntityFrameworkCore;

namespace DragonFarmApi
{
    public class DragonFarmContext : DbContext
    {
        public DragonFarmContext(DbContextOptions<DragonFarmContext> options)
            : base(options) {}
        
    }
}
