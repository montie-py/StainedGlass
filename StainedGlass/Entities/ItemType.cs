using StainedGlass.Entities.Transfer;

namespace StainedGlass.Entities;

public class ItemType : Entity
{
    public string Name { get; set; }
    public string Slug { get; set; }

    public void Save()
    {
        EntitiesCollection.ItemsTypes.Add(this.Slug, (ItemType)this);
    }
}