using DragonFarmApi.Models;
using DragonFarmApi.Orchestrators.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DragonFarmApi.Controllers;
/// <summary>
/// Controller for managing dragons with role-based access control
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Authorize] // Require authentication for all endpoints
public class DragonsController : ControllerBase
{
    private readonly ILogger<DragonsController> _logger;
    private readonly IDragonOrchestrator _dragonOrchestrator;

    public DragonsController(IDragonOrchestrator dragonOrchestrator, ILogger<DragonsController> logger)
    {
        _dragonOrchestrator = dragonOrchestrator;
        _logger = logger;
    }

    /// <summary>
    /// Get all dragons (User role and above)
    /// </summary>
    /// <returns>List of all dragons</returns>
    [HttpGet]
    [Authorize(Roles = "User,Manager,Admin")]
    public async Task<ActionResult<IEnumerable<Dragon>>> GetDragons()
    {
        try
        {
            var dragons = await _dragonOrchestrator.GetAllDragonsAsync();
            return Ok(dragons);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving dragons");
            return StatusCode(500, "An error occurred while retrieving dragons");
        }
    }

    /// <summary>
    /// Get a specific dragon by ID (User role and above)
    /// </summary>
    /// <param name="id">Dragon ID</param>
    /// <returns>Dragon details</returns>
    [HttpGet("{id}")]
    [Authorize(Roles = "User,Manager,Admin")]
    public async Task<ActionResult<Dragon>> GetDragon(Guid id)
    {
        try
        {
            var dragon = await _dragonOrchestrator.GetDragonByIdAsync(id);

            if (dragon == null)
            {
                return NotFound($"Dragon with ID {id} not found");
            }

            return Ok(dragon);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving dragon {DragonId}", id);
            return StatusCode(500, "An error occurred while retrieving the dragon");
        }
    }
}