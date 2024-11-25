using StainedGlass.Entities.Transfer;

namespace StainedGlass.Entities;

internal class Church : Entity
{
    public required string Slug {get; set;}
    public string Name {get; set;}
    public string Description {get; set;}
    public string Image {get; set;}
    public HashSet<SanctuarySide>? Sides { get; set; } = new();

    public void Save()
    {
        EntitiesCollection.Churches.TryAdd(Slug, this);
    }

    public void Replace(string slug, Entity entity)
    {
        entity.Slug = slug;
        var oldEntity = EntitiesCollection.Churches[slug];
        EntitiesCollection.Churches[slug] = (Church)entity;
        EntitiesCollection.Churches[slug].Sides = oldEntity.Sides;
    }

    public void Remove(string slug)
    {
        EntitiesCollection.Churches.Remove(slug);
    }
}