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
    public class FeedingRecordsController : ControllerBase
    {
        private readonly DragonFarmContext _context;

        public FeedingRecordsController(DragonFarmContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Gets all feeding records
        /// </summary>
        /// <returns>List of feeding records</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<FeedingRecordDto>))]
        public async Task<ActionResult<IEnumerable<FeedingRecordDto>>> GetFeedingRecords()
        {
            var feedingRecords = await _context.FeedingRecords
                .Include(fr => fr.Dragon)
                .Select(fr => new FeedingRecordDto
                {
                    Id = fr.Id,
                    DragonId = fr.DragonId,
                    DragonName = fr.Dragon.Name,
                    FoodType = fr.FoodType,
                    Amount = fr.Amount,
                    FeedingTime = fr.FeedingTime,
                    Notes = fr.Notes
                })
                .ToListAsync();

            return Ok(feedingRecords);
        }

        /// <summary>
        /// Gets feeding records for a specific dragon
        /// </summary>
        /// <param name="dragonId">Dragon ID</param>
        /// <returns>List of feeding records for the dragon</returns>
        [HttpGet("dragon/{dragonId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<FeedingRecordDto>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<FeedingRecordDto>>> GetFeedingRecordsByDragon(int dragonId)
        {
            var dragonExists = await _context.Dragons.AnyAsync(d => d.Id == dragonId);
            if (!dragonExists)
            {
                return NotFound($"Dragon with ID {dragonId} not found.");
            }

            var feedingRecords = await _context.FeedingRecords
                .Include(fr => fr.Dragon)
                .Where(fr => fr.DragonId == dragonId)
                .Select(fr => new FeedingRecordDto
                {
                    Id = fr.Id,
                    DragonId = fr.DragonId,
                    DragonName = fr.Dragon.Name,
                    FoodType = fr.FoodType,
                    Amount = fr.Amount,
                    FeedingTime = fr.FeedingTime,
                    Notes = fr.Notes
                })
                .ToListAsync();

            return Ok(feedingRecords);
        }

        /// <summary>
        /// Gets a specific feeding record by ID
        /// </summary>
        /// <param name="id">Feeding record ID</param>
        /// <returns>Feeding record details</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(FeedingRecordDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<FeedingRecordDto>> GetFeedingRecord(int id)
        {
            var feedingRecord = await _context.FeedingRecords
                .Include(fr => fr.Dragon)
                .FirstOrDefaultAsync(fr => fr.Id == id);

            if (feedingRecord == null)
            {
                return NotFound($"Feeding record with ID {id} not found.");
            }

            var feedingRecordDto = new FeedingRecordDto
            {
                Id = feedingRecord.Id,
                DragonId = feedingRecord.DragonId,
                DragonName = feedingRecord.Dragon.Name,
                FoodType = feedingRecord.FoodType,
                Amount = feedingRecord.Amount,
                FeedingTime = feedingRecord.FeedingTime,
                Notes = feedingRecord.Notes
            };

            return Ok(feedingRecordDto);
        }

        /// <summary>
        /// Creates a new feeding record
        /// </summary>
        /// <param name="createFeedingRecordDto">Feeding record details</param>
        /// <returns>Created feeding record</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(FeedingRecordDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<FeedingRecordDto>> CreateFeedingRecord(CreateFeedingRecordDto createFeedingRecordDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var dragon = await _context.Dragons.FindAsync(createFeedingRecordDto.DragonId);
            if (dragon == null)
            {
                return NotFound($"Dragon with ID {createFeedingRecordDto.DragonId} not found.");
            }

            var feedingRecord = new FeedingRecord
            {
                DragonId = createFeedingRecordDto.DragonId,
                FoodType = createFeedingRecordDto.FoodType,
                Amount = createFeedingRecordDto.Amount,
                Notes = createFeedingRecordDto.Notes,
                FeedingTime = DateTime.UtcNow
            };

            _context.FeedingRecords.Add(feedingRecord);
            await _context.SaveChangesAsync();

            var feedingRecordDto = new FeedingRecordDto
            {
                Id = feedingRecord.Id,
                DragonId = feedingRecord.DragonId,
                DragonName = dragon.Name,
                FoodType = feedingRecord.FoodType,
                Amount = feedingRecord.Amount,
                FeedingTime = feedingRecord.FeedingTime,
                Notes = feedingRecord.Notes
            };

            return CreatedAtAction(nameof(GetFeedingRecord), new { id = feedingRecord.Id }, feedingRecordDto);
        }

        /// <summary>
        /// Deletes a feeding record
        /// </summary>
        /// <param name="id">Feeding record ID</param>
        /// <returns>No content</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteFeedingRecord(int id)
        {
            var feedingRecord = await _context.FeedingRecords.FindAsync(id);

            if (feedingRecord == null)
            {
                return NotFound($"Feeding record with ID {id} not found.");
            }

            _context.FeedingRecords.Remove(feedingRecord);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}