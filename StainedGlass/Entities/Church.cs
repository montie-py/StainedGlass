namespace StainedGlass.Entities;

internal class Church : Entity
{
    internal required string Slug {get; set;}
    internal required string Name {get; set;}
    internal required string Image {get; set;}
    internal required HashSet<SanctuarySide> Sides {get; set;}
}