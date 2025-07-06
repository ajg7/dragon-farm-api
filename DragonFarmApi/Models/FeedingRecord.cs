namespace DragonFarmApi.Models
{
    public class FeedingRecord
    {
        public int Id { get; set; }
        public int DragonId { get; set; }
        public string FoodType { get; set; } = string.Empty;
        public double Amount { get; set; }
        public DateTime FeedingTime { get; set; } = DateTime.UtcNow;
        public string? Notes { get; set; }
        
        // Navigation property
        public Dragon Dragon { get; set; } = null!;
    }
}