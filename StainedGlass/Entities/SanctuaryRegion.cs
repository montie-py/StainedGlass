using StainedGlass.Entities.Transfer;

namespace StainedGlass.Entities;
internal class SanctuaryRegion : Entity
{
    public required string Slug {get; set;}
    public required string Name {get; set;}
    public required string Description {get; set;}
    public required string Image {get; set;}
    public required HashSet<Item>? Items { get; set; } = new();
    public required SanctuarySide? SanctuarySide {get; set;}

    public void Save()
    {
        EntitiesCollection.SanctuaryRegions.TryAdd(Slug, this);
    }

    public void Replace(string slug, Entity entity)
    {
        entity.Slug = slug;
        var oldEntity = EntitiesCollection.SanctuaryRegions[slug];
        EntitiesCollection.SanctuaryRegions[slug] = (SanctuaryRegion)entity;
        EntitiesCollection.SanctuaryRegions[slug].SanctuarySide = oldEntity.SanctuarySide;
        EntitiesCollection.SanctuaryRegions[slug].Items = oldEntity.Items;
    }

    public void Remove(string slug)
    {
        //remove region from its side
        SanctuarySide sanctuarySide = EntitiesCollection.SanctuarySides.Values.FirstOrDefault(e => 
            e.Regions?.FirstOrDefault(r => r.Slug == slug) != null
            );
        if (sanctuarySide != null)
        {
            var iterator = sanctuarySide.Regions.GetEnumerator();
            while (iterator.MoveNext())
            {
                var currentSide = iterator.Current;
                if (currentSide.Slug == slug)
                {
                    sanctuarySide.Regions.Remove(iterator.Current);
                }
            }
        }
        
        EntitiesCollection.SanctuaryRegions.Remove(slug);
    }

    public override bool Equals(object obj)
    {
        if (obj == null || GetType() != obj.GetType()) return false;

        SanctuaryRegion? other = obj as SanctuaryRegion;

        return Slug == other.Slug && Name == other.Name;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Slug, Name); 
    }

}
