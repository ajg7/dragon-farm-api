using DragonFarmApi.Models;

namespace DragonFarmApi.Repositories.Interfaces;
public interface IDragonRepository
{
     Task<List<Dragon>> GetAllDragonsAsync();
     Task<Dragon?> GetDragonByIdAsync(Guid id);
     Task<Dragon> CreateDragonAsync(Dragon dragon);
     Task<Dragon?> UpdateDragonAsync(Guid id, Dragon dragon);
     Task<bool> DeleteDragonAsync(Guid id);
}
