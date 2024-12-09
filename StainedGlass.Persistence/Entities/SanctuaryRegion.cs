using System.ComponentModel.DataAnnotations;

namespace StainedGlass.Persistence.Entities;
internal class SanctuaryRegion : IEntity
{
    [Key]
    public string Slug {get; set;}
    public string Name {get; set;}
    public string Description {get; set;}
    public string Image {get; set;}
    public string SanctuarySideSlug {get; set;}
    public SanctuarySide? SanctuarySide {get; set;}
    public ICollection<Item>? Items { get; set; }

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
