namespace DragonFarmApi.Models;
public class Trait
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public char DominantAllele { get; set; }
    public char RecessiveAllele { get; set; }
}
