namespace StainedGlass.Entities;

public class SanctuarySide
{
    public string Slug { get; set; }
    public string Name { get; set; }
    public List<SanctuaryRegion> Regions { get; set; }
}