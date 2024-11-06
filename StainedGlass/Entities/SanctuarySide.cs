namespace StainedGlass.Entities;

public class SanctuarySide : Entity
{
    public required string Slug { get; set; }
    public required string Name { get; set; }
    public required List<SanctuaryRegion> Regions { get; set; }
    public required Church Church { get; set; }
}