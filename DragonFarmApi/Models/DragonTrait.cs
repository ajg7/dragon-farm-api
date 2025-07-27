namespace DragonFarmApi.Models;
public class DragonTrait
{
    public Guid DragonId { get; set; }
    public int TraitId { get; set; }
    public char AlleleA { get; set; }
    public char AlleleB { get; set; }

    public virtual Dragon? Dragon { get; set; }
    public virtual Trait? Trait { get; set; }
}

