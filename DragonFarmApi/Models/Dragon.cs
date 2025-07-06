namespace DragonFarmApi.Models
{
    public class Dragon
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Species { get; set; } = string.Empty;
        public string Color { get; set; } = string.Empty;
        public int Age { get; set; }
        public double Weight { get; set; }
        public bool IsHealthy { get; set; } = true;
        public DateTime DateAcquired { get; set; } = DateTime.UtcNow;
        public string? Description { get; set; }
        
        // Navigation property
        public List<FeedingRecord> FeedingRecords { get; set; } = new();
    }
}