using StainedGlass.Entities;
using StainedGlass.Entities.Transfer;
using StainedGlass.Transfer.DTOs;

namespace StainedGlass.Transfer.Mapper;

internal class SanctuaryRegionMapper : Mappable
{
    
    public Transferable? GetDTO(Entity? entity)
     {
        if (entity == null) 
        {
            return null;
        }
        StainedGlassMapper stainedGlassMapper = new();
        SanctuarySideMapper sanctuarySideMapper = new();

        SanctuaryRegion region = entity as SanctuaryRegion;
        HashSet<StainedGlassDTO> WindowsDTOs = new();
        foreach (Entities.StainedGlass window in region.Windows)
        {
            WindowsDTOs.Add(stainedGlassMapper.GetDTO(window) as StainedGlassDTO);
        }

        SanctuarySideDTO sanctuarySideDTO = sanctuarySideMapper.GetDTO(region.SanctuarySide) as SanctuarySideDTO;
        
        var sanctuaryRegionDTO = new SanctuaryRegionDTO
        {
            Name = region.Name,
            Slug = region.Slug,
            Image = region.Image,
            Windows = WindowsDTOs,
            SanctuarySide = sanctuarySideDTO,
        };

        if (sanctuarySideDTO != null)
        {
            sanctuaryRegionDTO.SanctuarySideSlug = sanctuarySideDTO.Slug;
        }

        return sanctuaryRegionDTO;
     }

    public Transferable GetDTOBySlug(string slug)
    {
        return GetDTO(EntitiesCollection.SanctuaryRegions.FirstOrDefault(e => e.Slug.Equals(slug)));
    }

    public Entity GetEntity(Transferable transferable)
     {
        SanctuaryRegionDTO sanctuaryRegionDTO = transferable as SanctuaryRegionDTO;

        SanctuarySide sanctuarySide = EntitiesCollection.SanctuarySides.FirstOrDefault(
            s => s.Slug.Equals(sanctuaryRegionDTO.Slug)
            );
        
        SanctuaryRegion sanctuaryRegion = new SanctuaryRegion
        {
            Name = sanctuaryRegionDTO.Name,
            Slug = sanctuaryRegionDTO.Slug,
            Image = sanctuaryRegionDTO.Image,
            Windows = null,
            SanctuarySide = sanctuarySide,
        };
        
        if (sanctuaryRegionDTO.Windows != null)
        {
            var stainedGlassMapper = new StainedGlassMapper();
            HashSet<Entities.StainedGlass> windows = new();
            // sanctuaryRegion.Windows = (HashSet<Entities.StainedGlass>)sanctuaryRegionDTO.Windows.Select(
            //     e => (Entities.StainedGlass)stainedGlassMapper.GetEntity(e)
            //     );
            sanctuaryRegion.Windows = sanctuaryRegionDTO.Windows.Select(
                e => stainedGlassMapper.GetEntity(e) as Entities.StainedGlass
                ).ToHashSet();
        }

        if (sanctuarySide != null)
        {
            sanctuarySide.Regions.Add(sanctuaryRegion);
        }

        return sanctuaryRegion;
     }
}