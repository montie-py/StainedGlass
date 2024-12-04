namespace StainedGlass.Persistence.Entities;

internal class ItemType : IEntity
{
    public required string Name { get; set; }
    public required string Slug { get; set; }
    public ICollection<Item> Items { get; set; }
}