using StainedGlass.Entities;
using StainedGlass.Entities.Transfer;
using StainedGlass.Transfer.Mapper;

namespace StainedGlass.Transfer.DTOs;

public class SanctuarySideDTO : Transferable
{
    public required string Slug { get; set; }
    public required string Name { get; set; }
    public required List<SanctuaryRegionDTO> Regions { get; set; }
    public required string ChurchSlug { get; set; }
    public required ChurchDTO? Church { get; set; }

    public Entity GetEntity(Transferable transferable)
    {
        return (new SanctuarySideMapper()).GetEntity(transferable);
    }
}