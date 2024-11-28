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
        EntitiesCollection.SanctuarySides.TryAdd(Slug, this);
    }

    public void Replace(string slug, Entity entity)
    {
        if (!EntitiesCollection.SanctuarySides.ContainsKey(slug))
        {
            return;
        }
        entity.Slug = slug;
        var oldEntity = EntitiesCollection.SanctuarySides[slug];
        EntitiesCollection.SanctuarySides[slug] = (SanctuarySide)entity;
        
        //if old entity has an assigned church - new one cannot lack one
        if (EntitiesCollection.SanctuarySides[slug].Church is null)
        {
            EntitiesCollection.SanctuarySides[slug].Church = oldEntity.Church;
        }
        
        EntitiesCollection.SanctuarySides[slug].Regions = oldEntity.Regions;
    }

    public void Remove()
    {
        //remove the side from its church
        var church = EntitiesCollection.Churches.Values.FirstOrDefault(e => 
            e.Sides?.FirstOrDefault(s => s.Slug == Slug) != null
            );
        if (church != null)
        {
            church.Sides?.RemoveWhere(e => e.Slug == Slug);
        }
        EntitiesCollection.SanctuarySides.Remove(Slug);
    }
}