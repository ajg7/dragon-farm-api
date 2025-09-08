namespace DragonFarmApi.DTOs.Requests
{
    public class BreedingRequest
    {
        public Guid Id { get; set; }
        public Guid ParentAId { get; set; }
        public Guid ParentBId { get; set; }
        public DateTime RequestedAt { get; set; }
        public string Status { get; set; } = "Queued";
    }
}
