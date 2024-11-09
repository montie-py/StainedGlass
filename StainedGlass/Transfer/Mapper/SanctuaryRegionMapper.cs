using StainedGlass.Entities;
using StainedGlass.Entities.Transfer;
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

        var sanctuarySideDTO = sanctuarySideMapper.GetDTO(region.SanctuarySide) as SanctuarySideDTO;
        
        return new SanctuaryRegionDTO
        {
            Name = region.Name,
            Slug = region.Slug,
            Image = region.Image,
            Windows = WindowsDTOs,
            SanctuarySide = sanctuarySideDTO,
            SanctuarySideSlug = sanctuarySideDTO.Slug,
        };
     }

    public Transferable GetDTOBySlug(string slug)
    {
        return GetDTO(EntitiesCollection.SanctuaryRegions.FirstOrDefault(e => e.Slug.Equals(slug)));
    }

    public Entity GetEntity(Transferable transferable)
     {
        SanctuaryRegionDTO sanctuaryRegionDTO = transferable as SanctuaryRegionDTO;

        SanctuarySide sanctuarySide = EntitiesCollection.SanctuarySides.FirstOrDefault(s => s.Slug.Equals(sanctuaryRegionDTO.Slug));

        SanctuaryRegion sanctuaryRegion = new SanctuaryRegion
        {
            Name = sanctuaryRegionDTO.Name,
            Slug = sanctuaryRegionDTO.Slug,
            Image = sanctuaryRegionDTO.Image,
            Windows = null,
            SanctuarySide = sanctuarySide,
        };

        if (sanctuarySide != null)
        {
            sanctuarySide.Regions.Add(sanctuaryRegion);
        }

        return sanctuaryRegion;
     }
}