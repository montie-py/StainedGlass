using StainedGlass.Entities;
using StainedGlass.Entities.Transfer;
using StainedGlass.Transfer.DTOs;

namespace StainedGlass.Transfer.Mapper;

internal class SanctuarySideMapper : Mappable
{    
    public Transferable? GetDTO(Entity? entity)
    {
        if (entity == null) 
        {
            return null;
        }
        
        ChurchMapper churchMapper = new();
        SanctuaryRegionMapper sanctuaryRegionMapper = new();
        SanctuarySide? sanctuarySide = entity as SanctuarySide;
        List<SanctuaryRegionDTO> regionsDTO = new();
        foreach (SanctuaryRegion? region in sanctuarySide.Regions)
        {
            regionsDTO.Add(sanctuaryRegionMapper.GetDTO(region) as SanctuaryRegionDTO);
        }

        var churchDTO = churchMapper.GetDTO(sanctuarySide.Church) as ChurchDTO;
        
        var sanctuarySideDTO = new SanctuarySideDTO
        {
            Slug = sanctuarySide.Slug,
            Name = sanctuarySide.Name,
            Regions = regionsDTO,
            Church = churchDTO
        };

        if (churchDTO != null)
        {
            sanctuarySideDTO.ChurchSlug = churchDTO.Slug;
        }
        
        return sanctuarySideDTO;
    }

    public Transferable GetDTOBySlug(string slug)
    {
        return GetDTO(EntitiesCollection.SanctuarySides.FirstOrDefault(e => e.Slug.Equals(slug)));
    }

    public Entity GetEntity(Transferable transferable)
    {
        SanctuarySideDTO sanctuarySideDTO = transferable as SanctuarySideDTO;

        var sanctuarySideChurch = EntitiesCollection.Churches.FirstOrDefault(
            e => e.Slug.Equals(sanctuarySideDTO.ChurchSlug)
            );

        SanctuarySide sanctuarySide = new SanctuarySide
        {
            Name = sanctuarySideDTO.Name,
            Slug = sanctuarySideDTO.Slug,
            Regions = null,
            Church = sanctuarySideChurch,
        };

        if (sanctuarySideDTO.Regions != null)
        {
            SanctuaryRegionMapper sanctuaryRegionMapper = new();
            sanctuarySide.Regions =
                sanctuarySideDTO.Regions.Select(
                    e => sanctuaryRegionMapper.GetEntity(e) as SanctuaryRegion
                    ).ToList();
        }

        if (sanctuarySideChurch != null) {
            sanctuarySideChurch.Sides.Add(sanctuarySide);
        }

        return sanctuarySide;
    }
}