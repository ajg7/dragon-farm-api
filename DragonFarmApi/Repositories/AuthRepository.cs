using DragonFarmApi.Models;
using DragonFarmApi.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DragonFarmApi.Repositories;
public class AuthRepository : IAuthRepository
{
    private readonly ILogger<AuthRepository> _logger;
    private readonly DragonFarmContext _dragonFarmContext;
    public AuthRepository(ILogger<AuthRepository> logger, DragonFarmContext dragonFarmContext)
    {
        _logger = logger;
        _dragonFarmContext = dragonFarmContext;
    }
    public async Task<List<Role>> GetAllRolesAsync()
    {
        try
        {
            // Map identity roles to the public Role model
            var roles = await _dragonFarmContext.Roles
                .AsNoTracking()
                .Select(r => new Role
                {
                    CreatedAt = r.CreatedAt,
                    RoleName = r.Name ?? string.Empty,
                    Description = r.Description
                })
                .OrderBy(r => r.RoleName)
                .ToListAsync();

            return roles;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving roles from database");
            throw;
        }
    }
}

