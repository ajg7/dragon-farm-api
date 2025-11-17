using DragonFarmApi.Models;
using DragonFarmApi.Orchestrators.Interfaces;
using DragonFarmApi.Repositories.Interfaces;

namespace DragonFarmApi.Orchestrators;
public class DragonOrchestrator : IDragonOrchestrator
{
    private readonly ILogger<DragonOrchestrator> _logger;
    private readonly IDragonRepository _dragonRepository;

    public DragonOrchestrator(ILogger<DragonOrchestrator> logger, IDragonRepository dragonRepository)
    {
        _logger = logger;
        _dragonRepository = dragonRepository;
    }

    public async Task<List<Dragon>> GetAllDragonsAsync()
    {
        return await _dragonRepository.GetAllDragonsAsync();
    }

    public async Task<Dragon?> GetDragonByIdAsync(Guid id)
    {
        return await _dragonRepository.GetDragonByIdAsync(id);
    }

    public async Task<Dragon> CreateDragonAsync(Dragon dragon)
    {
        return await _dragonRepository.CreateDragonAsync(dragon);
    }

    public async Task<Dragon?> UpdateDragonAsync(Guid id, Dragon dragon)
    {
        return await _dragonRepository.UpdateDragonAsync(id, dragon);
    }

    public async Task<bool> DeleteDragonAsync(Guid id)
    {
        return await _dragonRepository.DeleteDragonAsync(id);
    }
}
