namespace StainedGlass.Transfer.DTOs;

public class SanctuarySideDTO : Transferable
{
    public required string Slug { get; set; }
    public required string Name { get; set; }
    public required ChurchDTO ChurchDTO { get; set; }
    public required List<SanctuaryRegionDTO> Regions { get; set; }
}