using DragonFarmApi.Models;

namespace DragonFarmApi.Repositories.Interfaces;
public interface IAuthRepository
{
    public Task<List<Role>> GetAllRolesAsync();
}
