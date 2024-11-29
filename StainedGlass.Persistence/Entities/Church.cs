

namespace StainedGlass.Persistence.Entities;

internal class Church : IEntity
{
    public required string Slug {get; set;}
    public string Name {get; set;}
    public string Description {get; set;}
    public string Image {get; set;}
    public ICollection<SanctuarySide>? SanctuarySides { get; set; }

    public void Save()
    {
        EntitiesCollection.Churches.TryAdd(Slug, this);
    }

    public void Replace(string slug, IEntity entity)
    {
        if (!EntitiesCollection.Churches.ContainsKey(slug))
        {
            return;
        }
        entity.Slug = slug;
        var oldEntity = EntitiesCollection.Churches[slug];
        EntitiesCollection.Churches[slug] = (Church)entity;
        EntitiesCollection.Churches[slug].Sides = oldEntity.Sides;
    }

    public void Remove()
    {
        EntitiesCollection.Churches.Remove(Slug);
    }
}