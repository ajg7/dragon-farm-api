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
}
