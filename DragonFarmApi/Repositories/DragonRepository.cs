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

    public async Task<Dragon> CreateDragonAsync(Dragon dragon)
    {
        try
        {
            _dragonFarmContext.Dragons.Add(dragon);
            var saveResults = await _dragonFarmContext.SaveChangesAsync();
            if (saveResults > 0) return dragon;
            else throw new Exception("Failed to create dragon");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating new dragon in database");
            throw;
        }
    }

    public async Task<Dragon?> UpdateDragonAsync(Guid id, Dragon dragon)
    {
        try
        {
            var existingDragon = await _dragonFarmContext.Dragons.FindAsync(id);
            if (existingDragon == null) return null;
            existingDragon.Name = dragon.Name;
            _dragonFarmContext.Dragons.Update(existingDragon);
            var saveResults = await _dragonFarmContext.SaveChangesAsync();
            if (saveResults > 0) return existingDragon;
            else throw new Exception("Failed to update dragon");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error updating dragon with ID {id} in database");
            throw;
        }
    }

    public async Task<bool> DeleteDragonAsync(Guid id)
    {
        try
        {
            var existingDragon = await _dragonFarmContext.Dragons.FindAsync(id);
            if (existingDragon == null) return false;
            _dragonFarmContext.Dragons.Remove(existingDragon);
            var saveResults = await _dragonFarmContext.SaveChangesAsync();
            return saveResults > 0;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error deleting dragon with ID {id} from database");
            throw;
        }
    }
}
