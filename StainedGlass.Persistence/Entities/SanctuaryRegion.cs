namespace StainedGlass.Persistence.Entities;
internal class SanctuaryRegion : IEntity
{
    public string Slug {get; set;}
    public string Name {get; set;}
    public string Description {get; set;}
    public string Image {get; set;}
    public string SanctuarySideSlug {get; set;}
    public SanctuarySide? SanctuarySide {get; set;}
    public ICollection<Item>? Items { get; set; }


    public void Save()
    {
        EntitiesCollection.SanctuaryRegions.TryAdd(Slug, this);
    }

    public void Replace(string slug, IEntity entity)
    {
        if (!EntitiesCollection.SanctuaryRegions.ContainsKey(slug))
        {
            return;
        }
        entity.Slug = slug;
        var oldEntity = EntitiesCollection.SanctuaryRegions[slug];
        EntitiesCollection.SanctuaryRegions[slug] = (SanctuaryRegion)entity;
        //if old entity has an assigned side - new one cannot lack one
        if (EntitiesCollection.SanctuaryRegions[slug].SanctuarySide is null)
        {
            EntitiesCollection.SanctuaryRegions[slug].SanctuarySide = oldEntity.SanctuarySide;
        }
        EntitiesCollection.SanctuaryRegions[slug].Items = oldEntity.Items;
    }

    public void Remove()
    {
        //remove region from its side
        var sanctuarySide = EntitiesCollection.SanctuarySides.Values.FirstOrDefault(e => 
            e.Regions?.FirstOrDefault(r => r.Slug == Slug) != null
            );
        if (sanctuarySide != null)
        {
            sanctuarySide.Regions?.RemoveAll(e => e.Slug == Slug);
        }
        
        EntitiesCollection.SanctuaryRegions.Remove(Slug);
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