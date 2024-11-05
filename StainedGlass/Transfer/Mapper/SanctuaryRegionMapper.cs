using StainedGlass.Entities;
using StainedGlass.Transfer.DTOs;

namespace StainedGlass.Transfer.Mapper;

internal class SanctuaryRegionMapper : Mappable
{
    StainedGlassMapper stainedGlassMapper = new();
    SanctuarySideMapper sanctuarySideMapper = new();
    
    public Transferable GetDTO(Entity entity)
     {
        SanctuaryRegion region = entity as SanctuaryRegion;
        HashSet<StainedGlassDTO> WindowsDTOs = new();
        foreach (Entities.StainedGlass window in region.Windows)
        {
            WindowsDTOs.Add(stainedGlassMapper.GetDTO(window) as StainedGlassDTO);
        }
        
        return new SanctuaryRegionDTO
        {
            Name = region.Name,
            Slug = region.Slug,
            Image = region.Image,
            Windows = WindowsDTOs,
            SanctuarySideDTO = sanctuarySideMapper.GetDTO(region.SanctuarySide) as SanctuarySideDTO,
        };
     }

     public Entity GetEntity(Transferable transferable)
     {
        SanctuaryRegionDTO sanctuaryRegionDTO = transferable as SanctuaryRegionDTO;
        HashSet<Entities.StainedGlass> Windows = new();

        foreach(StainedGlassDTO stainedGlassDTO in sanctuaryRegionDTO.Windows)
        {
            Windows.Add(stainedGlassMapper.GetEntity(stainedGlassDTO) as Entities.StainedGlass);
        }

        return new SanctuaryRegion
        {
            Name = sanctuaryRegionDTO.Name,
            Slug = sanctuaryRegionDTO.Slug,
            Image = sanctuaryRegionDTO.Image,
            Windows = Windows,
            SanctuarySide = sanctuarySideMapper.GetEntity(sanctuaryRegionDTO.SanctuarySideDTO)  as SanctuarySide,
        };
     }
}