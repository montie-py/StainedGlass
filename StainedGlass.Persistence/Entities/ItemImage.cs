using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StainedGlass.Persistence.Entities;

[Table("ItemImages")]
internal class ItemImage : IEntity
{
    [Key]
    public string Slug { get; set; }
    public string ItemSlug { get; set; }
    public Item? Item { get; set; }
    public byte[] Image { get; set; }
}