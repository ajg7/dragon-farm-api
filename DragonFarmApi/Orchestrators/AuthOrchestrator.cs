using DragonFarmApi.Models;
using DragonFarmApi.Orchestrators.Interfaces;
using DragonFarmApi.Repositories.Interfaces;

namespace DragonFarmApi.Orchestrators;
public class AuthOrchestrator : IAuthOrchestrator
{
    private readonly ILogger<AuthOrchestrator> _logger;
    private readonly IAuthRepository _authRepository;

    public AuthOrchestrator(ILogger<AuthOrchestrator> logger, IAuthRepository authRepository)
    {
        _logger = logger;
        _authRepository = authRepository;
    }

    public async Task<List<Role>> GetAllRolesAsync()
    {
        return await _authRepository.GetAllRolesAsync();
    }
}

