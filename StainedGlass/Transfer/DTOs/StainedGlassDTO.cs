namespace StainedGlass.Transfer.DTOs;

public class StainedGlassDTO : Transferable
{
    public required string Slug {get; set;}
    public required string Title{get; set;}
    public required string Description{get; set;}
    public required string Image{get; set;}
    public required string SanctuaryRegionSlug {get; set;}
    public required SanctuaryRegionDTO SanctuaryRegion {get; set;}
}