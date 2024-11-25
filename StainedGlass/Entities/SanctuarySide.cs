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
        entity.Slug = slug;
        var oldEntity = EntitiesCollection.SanctuarySides[slug];
        EntitiesCollection.SanctuarySides[slug] = (SanctuarySide)entity;
        EntitiesCollection.SanctuarySides[slug].Church = oldEntity.Church;
        EntitiesCollection.SanctuarySides[slug].Regions = oldEntity.Regions;
    }

    public void Remove(string slug)
    {
        //remove the side from its church
        Church church = EntitiesCollection.Churches.Values.FirstOrDefault(e => 
            e.Sides?.FirstOrDefault(s => s.Slug == slug) != null
            );
        if (church != null)
        {
            var iterator = church.Sides.GetEnumerator();
            while (iterator.MoveNext())
            {
                SanctuarySide sanctuarySide = iterator.Current;
                if (sanctuarySide.Slug == slug)
                {
                    church.Sides.Remove(sanctuarySide);
                }
            }
        }
        EntitiesCollection.SanctuarySides.Remove(slug);
    }
}