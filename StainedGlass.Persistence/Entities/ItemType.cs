using System.ComponentModel.DataAnnotations;

namespace StainedGlass.Persistence.Entities;

internal class ItemType : IEntity
{
    [Key]
    public required string Slug { get; set; }
    public required string Name { get; set; }

    public ICollection<Item>? Items { get; set; }
}