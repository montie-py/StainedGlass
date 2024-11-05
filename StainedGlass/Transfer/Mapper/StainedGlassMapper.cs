using StainedGlass.Entities;
using StainedGlass.Transfer.DTOs;

namespace StainedGlass.Transfer.Mapper;

internal class StainedGlassMapper : Mappable
{
    SanctuaryRegionMapper sanctuaryRegionMapper = new();

    public Transferable GetDTO(Entity entity)
    {
        Entities.StainedGlass stainedGlass = entity as Entities.StainedGlass;
        return new StainedGlassDTO
        {
            Slug = stainedGlass.Slug,
            Title = stainedGlass.Title,
            Description = stainedGlass.Description,
            Image = stainedGlass.Image,
            SanctuaryRegionDTO = sanctuaryRegionMapper.GetDTO(stainedGlass.SanctuaryRegion) as SanctuaryRegionDTO
        };
    }

    public Entity GetEntity(Transferable transferable)
    {
        StainedGlassDTO stainedGlassDTO = transferable as StainedGlassDTO;
        return new Entities.StainedGlass
        {
            Slug = stainedGlassDTO.Slug,
            Title = stainedGlassDTO.Title,
            Description = stainedGlassDTO.Description,
            Image = stainedGlassDTO.Image,
            SanctuaryRegion = sanctuaryRegionMapper.GetEntity(stainedGlassDTO.SanctuaryRegionDTO) as SanctuaryRegion
        };
    }
}