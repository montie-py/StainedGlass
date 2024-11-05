using StainedGlass.Entities;
using StainedGlass.Transfer.DTOs;

namespace StainedGlass.Transfer.Mapper;

internal class SanctuarySideMapper : Mappable
{
    SanctuaryRegionMapper sanctuaryRegionMapper = new();
    ChurchMapper churchMapper = new();
    public Transferable GetDTO(Entity entity)
    {
        SanctuarySide? sanctuarySide = entity as SanctuarySide;
        List<SanctuaryRegionDTO> regionsDTO = new();
        foreach (SanctuaryRegion? region in sanctuarySide.Regions)
        {
            regionsDTO.Add(sanctuaryRegionMapper.GetDTO(region) as SanctuaryRegionDTO);
        }
        
        return new SanctuarySideDTO
        {
            Slug = sanctuarySide.Slug,
            Name = sanctuarySide.Name,
            Regions = regionsDTO,
            ChurchDTO = churchMapper.GetDTO(sanctuarySide.Church) as ChurchDTO
        };
    }

    public Entity GetEntity(Transferable transferable)
    {
        SanctuarySideDTO sanctuarySideDTO = transferable as SanctuarySideDTO;
        List<SanctuaryRegion> regions = new();
        foreach (var sanctuaryRegionDTO in sanctuarySideDTO.Regions)
        {
            regions.Add(sanctuaryRegionMapper.GetEntity(sanctuaryRegionDTO) as SanctuaryRegion);    
        }

        return new SanctuarySide
        {
            Name = sanctuarySideDTO.Name,
            Slug = sanctuarySideDTO.Slug,
            Church = churchMapper.GetEntity(sanctuarySideDTO.ChurchDTO) as Church,
            Regions = regions
        };
    }
}