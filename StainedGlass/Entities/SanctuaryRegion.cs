using StainedGlass.Entities.Transfer;

namespace StainedGlass.Entities;
internal class SanctuaryRegion : Entity
{
    public required string Slug {get; set;}
    public required string Name {get; set;}
    public required string Description {get; set;}
    public required string Image {get; set;}
    public required HashSet<Item>? Windows { get; set; } = new();
    public required SanctuarySide? SanctuarySide {get; set;}

    public void Save()
    {
        EntitiesCollection.SanctuaryRegions.Add(Slug, this);
    }

    public void Replace(string slug, Entity entity)
    {
        entity.Slug = slug;
        EntitiesCollection.SanctuaryRegions[slug] = (SanctuaryRegion)entity;
    }

    public void Remove(string slug)
    {
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
