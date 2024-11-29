namespace StainedGlass.Persistence.Entities;

public class ItemType : IEntity
{
    public string Name { get; set; }
    public string Slug { get; set; }

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