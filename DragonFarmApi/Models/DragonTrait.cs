namespace DragonFarmApi.Models;
public class DragonTrait
{
    public Guid DragonId { get; set; }
    public int TraitId { get; set; }
    public char AlleleA { get; set; }
    public char AlleleB { get; set; }

    public Dragon? Dragon { get; set; }
    public Trait? Trait { get; set; }
}

