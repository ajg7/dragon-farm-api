using DragonFarmApi.Models;

namespace DragonFarmApi.Orchestrators.Interfaces;
public interface IAuthOrchestrator
{
    public Task<List<Role>> GetAllRolesAsync();
}
