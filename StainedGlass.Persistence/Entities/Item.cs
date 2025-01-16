

using System.ComponentModel.DataAnnotations;

namespace StainedGlass.Persistence.Entities;
internal class Item : IEntity
{
    [Key]
    public string Slug {get; set;}
    public string Title{get; set;}
    public string Description{get; set;}
    public string Position {get; set;}
    public string ItemTypeSlug {get; set;}
    public ItemType ItemType{get; set;}
    public string SanctuaryRegionSlug {get; set;}
    public SanctuaryRegion SanctuaryRegion{get; set;}
    public ICollection<ItemRelation> RelatedItems {get; set; } = new List<ItemRelation>();
    public ICollection<ItemImage> ItemImages {get;} = new List<ItemImage>();

    public override bool Equals(object? obj)
    {
        if (obj == null || GetType() != obj.GetType()) return false;
        Item? other = obj as Item;
        return Slug == other.Slug && Title == other.Title && Description == other.Description;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Slug, Title, Description);
    }
}