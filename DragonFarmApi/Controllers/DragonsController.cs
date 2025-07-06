using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DragonFarmApi.Data;
using DragonFarmApi.Models;
using DragonFarmApi.DTOs;

namespace DragonFarmApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class DragonsController : ControllerBase
    {
        private readonly DragonFarmContext _context;

        public DragonsController(DragonFarmContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Gets all dragons in the farm
        /// </summary>
        /// <returns>List of dragons</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<DragonDto>))]
        public async Task<ActionResult<IEnumerable<DragonDto>>> GetDragons()
        {
            var dragons = await _context.Dragons
                .Select(d => new DragonDto
                {
                    Id = d.Id,
                    Name = d.Name,
                    Species = d.Species,
                    Color = d.Color,
                    Age = d.Age,
                    Weight = d.Weight,
                    IsHealthy = d.IsHealthy,
                    DateAcquired = d.DateAcquired,
                    Description = d.Description
                })
                .ToListAsync();

            return Ok(dragons);
        }

        /// <summary>
        /// Gets a specific dragon by ID
        /// </summary>
        /// <param name="id">Dragon ID</param>
        /// <returns>Dragon details</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(DragonDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<DragonDto>> GetDragon(int id)
        {
            var dragon = await _context.Dragons.FindAsync(id);

            if (dragon == null)
            {
                return NotFound($"Dragon with ID {id} not found.");
            }

            var dragonDto = new DragonDto
            {
                Id = dragon.Id,
                Name = dragon.Name,
                Species = dragon.Species,
                Color = dragon.Color,
                Age = dragon.Age,
                Weight = dragon.Weight,
                IsHealthy = dragon.IsHealthy,
                DateAcquired = dragon.DateAcquired,
                Description = dragon.Description
            };

            return Ok(dragonDto);
        }

        /// <summary>
        /// Creates a new dragon
        /// </summary>
        /// <param name="createDragonDto">Dragon details</param>
        /// <returns>Created dragon</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(DragonDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<DragonDto>> CreateDragon(CreateDragonDto createDragonDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var dragon = new Dragon
            {
                Name = createDragonDto.Name,
                Species = createDragonDto.Species,
                Color = createDragonDto.Color,
                Age = createDragonDto.Age,
                Weight = createDragonDto.Weight,
                IsHealthy = createDragonDto.IsHealthy,
                Description = createDragonDto.Description,
                DateAcquired = DateTime.UtcNow
            };

            _context.Dragons.Add(dragon);
            await _context.SaveChangesAsync();

            var dragonDto = new DragonDto
            {
                Id = dragon.Id,
                Name = dragon.Name,
                Species = dragon.Species,
                Color = dragon.Color,
                Age = dragon.Age,
                Weight = dragon.Weight,
                IsHealthy = dragon.IsHealthy,
                DateAcquired = dragon.DateAcquired,
                Description = dragon.Description
            };

            return CreatedAtAction(nameof(GetDragon), new { id = dragon.Id }, dragonDto);
        }

        /// <summary>
        /// Updates an existing dragon
        /// </summary>
        /// <param name="id">Dragon ID</param>
        /// <param name="updateDragonDto">Updated dragon details</param>
        /// <returns>Updated dragon</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(DragonDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<DragonDto>> UpdateDragon(int id, UpdateDragonDto updateDragonDto)
        {
            var dragon = await _context.Dragons.FindAsync(id);

            if (dragon == null)
            {
                return NotFound($"Dragon with ID {id} not found.");
            }

            if (updateDragonDto.Name != null) dragon.Name = updateDragonDto.Name;
            if (updateDragonDto.Species != null) dragon.Species = updateDragonDto.Species;
            if (updateDragonDto.Color != null) dragon.Color = updateDragonDto.Color;
            if (updateDragonDto.Age.HasValue) dragon.Age = updateDragonDto.Age.Value;
            if (updateDragonDto.Weight.HasValue) dragon.Weight = updateDragonDto.Weight.Value;
            if (updateDragonDto.IsHealthy.HasValue) dragon.IsHealthy = updateDragonDto.IsHealthy.Value;
            if (updateDragonDto.Description != null) dragon.Description = updateDragonDto.Description;

            await _context.SaveChangesAsync();

            var dragonDto = new DragonDto
            {
                Id = dragon.Id,
                Name = dragon.Name,
                Species = dragon.Species,
                Color = dragon.Color,
                Age = dragon.Age,
                Weight = dragon.Weight,
                IsHealthy = dragon.IsHealthy,
                DateAcquired = dragon.DateAcquired,
                Description = dragon.Description
            };

            return Ok(dragonDto);
        }

        /// <summary>
        /// Deletes a dragon
        /// </summary>
        /// <param name="id">Dragon ID</param>
        /// <returns>No content</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteDragon(int id)
        {
            var dragon = await _context.Dragons.FindAsync(id);

            if (dragon == null)
            {
                return NotFound($"Dragon with ID {id} not found.");
            }

            _context.Dragons.Remove(dragon);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}