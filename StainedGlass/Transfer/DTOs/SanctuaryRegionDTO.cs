using StainedGlass.Entities;
using StainedGlass.Transfer.Mapper;

namespace StainedGlass.Transfer.DTOs;

public class SanctuaryRegionDTO : Transferable
{
    public required string Slug {get; set;}
    public required string Name {get; set;}
    public required string Image {get; set;}
    public required HashSet<StainedGlassDTO>? Windows {get; set;}
    public required string SanctuarySideSlug {get; set;}
    public required SanctuarySideDTO SanctuarySide {get; set;}

    public Entity GetEntity(Transferable transferable)
     {
        return (new SanctuaryRegionMapper()).GetEntity(transferable); 
     }

    public Mappable GetMapper()
    {
        return new SanctuaryRegionMapper();
    }
}