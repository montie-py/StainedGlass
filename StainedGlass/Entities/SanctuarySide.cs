using StainedGlass.Entities.Transfer;

namespace StainedGlass.Entities;

internal class SanctuarySide : Entity
{
    public required string Slug { get; set; }
    public required string Name { get; set; }
    public required List<SanctuaryRegion>? Regions { get; set; } = new();
    public required Church? Church { get; set; }

    public void Save()
    {
        EntitiesCollection.SanctuarySides.Add(Slug, this);
    }

    public void Replace(string slug, Entity entity)
    {
        entity.Slug = slug;
        EntitiesCollection.SanctuarySides[slug] = (SanctuarySide)entity;
    }

    public void Remove(string slug)
    {
        EntitiesCollection.SanctuarySides.Remove(slug);
    }
}