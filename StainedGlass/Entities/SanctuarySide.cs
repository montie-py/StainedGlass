using StainedGlass.Entities.Transfer;

namespace StainedGlass.Entities;

internal class SanctuarySide : Entity
{
    public required string Slug { get; set; }
    public required string Name { get; set; }
    public required List<SanctuaryRegion>? Regions { get; set; }
    public required Church? Church { get; set; }

    public void Save()
    {
        EntitiesCollection.SanctuarySides.Add((SanctuarySide)this);
    }
}