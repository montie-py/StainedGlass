namespace StainedGlass.Persistence.Entities;

internal class SanctuarySide : IEntity
{
    public required string Slug { get; set; }
    public required string Name { get; set; }
    public ICollection<SanctuaryRegion>? Regions { get; set; }
    public Church? Church { get; set; }
    public required string ChurchSlug { get; set; }
}