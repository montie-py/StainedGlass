namespace StainedGlass.Entities;

internal class SanctuarySide : Entity
{
    internal required string Slug { get; set; }
    internal required string Name { get; set; }
    internal required List<SanctuaryRegion> Regions { get; set; }
    internal required Church Church { get; set; }
}