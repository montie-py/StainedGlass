namespace StainedGlass.Persistence.Entities;

internal class ItemType : IEntity
{
    public required string Name { get; set; }
    public required string Slug { get; set; }
    public ICollection<Item> Items { get; set; }

    public void Save()
    {
        EntitiesCollection.ItemsTypes.TryAdd(Slug, this);
    }

    public void Replace(string slug, IEntity entity)
    {
        if (!EntitiesCollection.ItemsTypes.ContainsKey(slug))
        {
            return;
        }
        entity.Slug = slug;
        EntitiesCollection.ItemsTypes[slug] = (ItemType)entity;
    }

    public void Remove()
    {
        EntitiesCollection.ItemsTypes.Remove(Slug);
    }
}