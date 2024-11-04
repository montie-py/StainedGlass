namespace StainedGlass.Entities;

public class SanctuarySide
{
    public required string Slug { get; set; }
    public required string Name { get; set; }
    public required List<SanctuaryRegion> Regions { get; set; }
}