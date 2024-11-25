using StainedGlass.Entities.Transfer;

namespace StainedGlass.Entities;

public class ItemType : Entity
{
    public string Name { get; set; }
    public string Slug { get; set; }

    public void Save()
    {
        EntitiesCollection.ItemsTypes.TryAdd(Slug, this);
    }

    public void Replace(string slug, Entity entity)
    {
        entity.Slug = slug;
        EntitiesCollection.ItemsTypes[slug] = (ItemType)entity;
    }

    public void Remove(string slug)
    {
        EntitiesCollection.ItemsTypes.Remove(slug);
    }
}