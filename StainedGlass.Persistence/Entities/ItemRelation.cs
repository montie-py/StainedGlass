namespace StainedGlass.Persistence.Entities;

internal class ItemRelation
{
    internal string ItemSlug { get; set; }
    internal Item Item { get; set; }
    internal string RelatedItemSlug { get; set; }
    internal Item RelatedItem { get; set; }
}