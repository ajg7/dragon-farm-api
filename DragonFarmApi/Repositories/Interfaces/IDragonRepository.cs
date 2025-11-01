using DragonFarmApi.Models;

namespace DragonFarmApi.Repositories.Interfaces;
public interface IDragonRepository
{
 Task<List<Dragon>> GetAllDragonsAsync();
 Task<Dragon?> GetDragonByIdAsync(Guid id);
}
