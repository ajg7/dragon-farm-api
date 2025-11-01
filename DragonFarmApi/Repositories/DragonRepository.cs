using DragonFarmApi.Models;
using Microsoft.EntityFrameworkCore;
using DragonFarmApi.Repositories.Interfaces;

namespace DragonFarmApi.Repositories;
public class DragonRepository : IDragonRepository
{
    private readonly ILogger<DragonRepository> _logger;
    private readonly DragonFarmContext _dragonFarmContext;
    public DragonRepository(ILogger<DragonRepository> logger, DragonFarmContext dragonFarmContext)
    {
        _logger = logger;
        _dragonFarmContext = dragonFarmContext;
    }

    public async Task<List<Dragon>> GetAllDragonsAsync()
    {
        try
        {
            return await _dragonFarmContext.Dragons
                .AsNoTracking()
                .Select(dragon => new Dragon
                {
                    Id = dragon.Id,
                    Name = dragon.Name,

                })
                .ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving dragons from database");
            throw;
        }
    }

    public async Task<Dragon?> GetDragonByIdAsync(Guid id)
    {
        try
        {
            return await _dragonFarmContext.Dragons
                .AsNoTracking()
                .Include(d => d.Traits)
                .ThenInclude(dt => dt.Trait)
                .FirstOrDefaultAsync(d => d.Id == id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error retrieving dragon with ID {id} from database");
            throw;
        }
    }
}
