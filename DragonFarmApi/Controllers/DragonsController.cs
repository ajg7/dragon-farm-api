using DragonFarmApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DragonFarmApi.Controllers;

/// <summary>
/// Controller for managing dragons with role-based access control
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Authorize] // Require authentication for all endpoints
public class DragonsController : ControllerBase
{
    private readonly DragonFarmContext _context;
    private readonly ILogger<DragonsController> _logger;

    public DragonsController(DragonFarmContext context, ILogger<DragonsController> logger)
    {
        _context = context;
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
            var dragons = await _context.Dragons
                .Include(d => d.Traits)
                .ThenInclude(dt => dt.Trait)
                .ToListAsync();

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
            var dragon = await _context.Dragons
                .Include(d => d.Traits)
                .ThenInclude(dt => dt.Trait)
                .FirstOrDefaultAsync(d => d.Id == id);

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
    /// Create a new dragon (Manager role and above)
    /// </summary>
    /// <param name="dragon">Dragon data</param>
    /// <returns>Created dragon</returns>
    [HttpPost]
    [Authorize(Roles = "Manager,Admin")]
    public async Task<ActionResult<Dragon>> CreateDragon(Dragon dragon)
    {
        try
        {
            if (dragon.Id == Guid.Empty)
            {
                dragon.Id = Guid.NewGuid();
            }

            if (dragon.HatchedAt == default)
            {
                dragon.HatchedAt = DateTimeOffset.UtcNow;
            }

            _context.Dragons.Add(dragon);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetDragon), new { id = dragon.Id }, dragon);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating dragon");
            return StatusCode(500, "An error occurred while creating the dragon");
        }
    }

    /// <summary>
    /// Update an existing dragon (Manager role and above)
    /// </summary>
    /// <param name="id">Dragon ID</param>
    /// <param name="dragon">Updated dragon data</param>
    /// <returns>Updated dragon</returns>
    [HttpPut("{id}")]
    [Authorize(Roles = "Manager,Admin")]
    public async Task<IActionResult> UpdateDragon(Guid id, Dragon dragon)
    {
        try
        {
            if (id != dragon.Id)
            {
                return BadRequest("Dragon ID mismatch");
            }

            var existingDragon = await _context.Dragons.FindAsync(id);
            if (existingDragon == null)
            {
                return NotFound($"Dragon with ID {id} not found");
            }

            existingDragon.Name = dragon.Name;
            existingDragon.Sex = dragon.Sex;
            existingDragon.RarityScore = dragon.RarityScore;
            // Note: HatchedAt should not be updated after creation

            await _context.SaveChangesAsync();

            return Ok(existingDragon);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating dragon {DragonId}", id);
            return StatusCode(500, "An error occurred while updating the dragon");
        }
    }

    /// <summary>
    /// Delete a dragon (Admin role only)
    /// </summary>
    /// <param name="id">Dragon ID</param>
    /// <returns>Success message</returns>
    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteDragon(Guid id)
    {
        try
        {
            var dragon = await _context.Dragons.FindAsync(id);
            if (dragon == null)
            {
                return NotFound($"Dragon with ID {id} not found");
            }

            _context.Dragons.Remove(dragon);
            await _context.SaveChangesAsync();

            return Ok($"Dragon {dragon.Name} deleted successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting dragon {DragonId}", id);
            return StatusCode(500, "An error occurred while deleting the dragon");
        }
    }

    /// <summary>
    /// Get dragon statistics (Admin role only)
    /// </summary>
    /// <returns>Dragon statistics</returns>
    [HttpGet("statistics")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<object>> GetDragonStatistics()
    {
        try
        {
            var totalDragons = await _context.Dragons.CountAsync();
            var maleCount = await _context.Dragons.CountAsync(d => d.Sex == DragonSex.Male);
            var femaleCount = await _context.Dragons.CountAsync(d => d.Sex == DragonSex.Female);
            var averageRarity = await _context.Dragons.AverageAsync(d => d.RarityScore);

            var statistics = new
            {
                TotalDragons = totalDragons,
                MaleCount = maleCount,
                FemaleCount = femaleCount,
                AverageRarityScore = Math.Round(averageRarity, 2),
                LastUpdated = DateTime.UtcNow
            };

            return Ok(statistics);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving dragon statistics");
            return StatusCode(500, "An error occurred while retrieving statistics");
        }
    }
}