using StainedGlass.Entities.Transfer;

namespace StainedGlass.Entities;

internal class Church : Entity
{
    public required string Slug {get; set;}
    public required string Name {get; set;}
    public required string Image {get; set;}
    public required HashSet<SanctuarySide?>? Sides {get; set;}

    public void Save()
    {
        EntitiesCollection.Churches.Add((Church)this);
    }
}