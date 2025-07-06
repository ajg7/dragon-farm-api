using System.ComponentModel.DataAnnotations;

namespace DragonFarmApi.DTOs
{
    public class FeedingRecordDto
    {
        public int Id { get; set; }
        public int DragonId { get; set; }
        public string DragonName { get; set; } = string.Empty;
        public string FoodType { get; set; } = string.Empty;
        public double Amount { get; set; }
        public DateTime FeedingTime { get; set; }
        public string? Notes { get; set; }
    }

    public class CreateFeedingRecordDto
    {
        [Required(ErrorMessage = "Dragon ID is required")]
        public int DragonId { get; set; }

        [Required(ErrorMessage = "Food type is required")]
        [StringLength(50, ErrorMessage = "Food type cannot exceed 50 characters")]
        public string FoodType { get; set; } = string.Empty;

        [Range(0.1, 1000, ErrorMessage = "Amount must be between 0.1 and 1000")]
        public double Amount { get; set; }

        [StringLength(200, ErrorMessage = "Notes cannot exceed 200 characters")]
        public string? Notes { get; set; }
    }
}