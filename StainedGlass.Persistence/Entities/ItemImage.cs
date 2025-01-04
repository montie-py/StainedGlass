using System.ComponentModel.DataAnnotations;

namespace StainedGlass.Persistence.Entities;

internal class ItemImage : IEntity
{
    [Key]
    public string Slug { get; set; }
    public string ItemSlug { get; set; }
    public Item? Item { get; set; }
    public byte[] Image { get; set; }
}