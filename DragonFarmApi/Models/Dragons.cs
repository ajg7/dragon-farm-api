namespace DragonFarmApi.Models;
public class Dragon
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public DragonSex Sex { get; set; }
    public DateTimeOffset HatchedAt { get; set; }
    public double RarityScore { get; set; }

    public ICollection<DragonTrait> Traits { get; set; } = [];
}
