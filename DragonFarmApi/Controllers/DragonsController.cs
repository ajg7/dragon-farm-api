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
    public async Task<ActionResult<List<Dragon>>> GetDragons()
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
    public async Task<ActionResult<Dragon>> GetDragonById(Guid id)
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

    /// <summary>
    /// Create a new dragon (User role and above)
    /// </summary>
    /// <param name="dragon">Dragon data to create</param>
    /// <returns>The created dragon with its assigned ID</returns>
    [HttpPost]
    [Authorize(Roles = "User,Manager,Admin")]
    public async Task<ActionResult<Dragon>> CreateDragon([FromBody] Dragon dragon)
    {
        try
        {
            var dragonResult = await _dragonOrchestrator.CreateDragonAsync(dragon);
            return CreatedAtAction(nameof(GetDragonById), new { id = dragonResult.Id }, dragonResult);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating dragon");
            return StatusCode(500, "An error occurred while creating the dragon");
        }
    }

    /// <summary>
    /// Update an existing dragon by ID (User role and above)
    /// </summary>
    /// <param name="id">ID of the dragon to update</param>
    /// <param name="dragon">Updated dragon data</param>
    /// <returns>The updated dragon, or404 if not found</returns>
    [HttpPut("{id}")]
    [Authorize(Roles = "User,Manager,Admin")]
    public async Task<ActionResult<Dragon>> UpdateDragon(Guid id, [FromBody] Dragon dragon)
    {
        var dragonResult = await _dragonOrchestrator.UpdateDragonAsync(id, dragon);
        if (dragonResult == null) return NotFound($"Dragon with ID {id} not found");
        return Ok(dragonResult);
    }

    /// <summary>
    /// Delete a dragon by ID (User role and above)
    /// </summary>
    /// <param name="id">ID of the dragon to delete</param>
    /// <returns>No content if deletion succeeds, or404 if not found</returns>
    [HttpDelete("{id}")]
    [Authorize(Roles = "User,Manager,Admin")]
    public async Task<ActionResult<Dragon>> DeleteDragon(Guid id)
    {
        var deleteResult = await _dragonOrchestrator.DeleteDragonAsync(id);
        if (!deleteResult) return NotFound($"Dragon with ID {id} not found");
        return NoContent();
    }
}