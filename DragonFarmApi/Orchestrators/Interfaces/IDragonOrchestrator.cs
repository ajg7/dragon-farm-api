using DragonFarmApi.Models;

namespace DragonFarmApi.Orchestrators.Interfaces;
public interface IDragonOrchestrator
{
 Task<List<Dragon>> GetAllDragonsAsync();
 Task<Dragon?> GetDragonByIdAsync(Guid id);
}
