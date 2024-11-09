using StainedGlass.Entities;
using StainedGlass.Entities.Transfer;
using StainedGlass.Transfer.DTOs;

namespace StainedGlass.Transfer.Mapper;

internal class SanctuarySideMapper : Mappable
{
    SanctuaryRegionMapper sanctuaryRegionMapper = new();
    ChurchMapper churchMapper = new();
    public Transferable GetDTO(Entity? entity)
    {
        SanctuarySide? sanctuarySide = entity as SanctuarySide;
        List<SanctuaryRegionDTO> regionsDTO = new();
        foreach (SanctuaryRegion? region in sanctuarySide.Regions)
        {
            regionsDTO.Add(sanctuaryRegionMapper.GetDTO(region) as SanctuaryRegionDTO);
        }

        var churchDTO = churchMapper.GetDTO(sanctuarySide.Church) as ChurchDTO;
        
        return new SanctuarySideDTO
        {
            Slug = sanctuarySide.Slug,
            Name = sanctuarySide.Name,
            Regions = regionsDTO,
            Church = churchDTO,
            ChurchSlug = churchDTO.Slug
        };
    }

    public Transferable GetDTOBySlug(string slug)
    {
        return GetDTO(EntitiesCollection.SanctuarySides.FirstOrDefault(e => e.Slug.Equals(slug)));
    }

    public Entity GetEntity(Transferable transferable)
    {
        SanctuarySideDTO sanctuarySideDTO = transferable as SanctuarySideDTO;

        var sanctuarySideChurch = EntitiesCollection.Churches.FirstOrDefault(e => e.Slug.Equals(sanctuarySideDTO.ChurchSlug));

        SanctuarySide sanctuarySide = new SanctuarySide
        {
            Name = sanctuarySideDTO.Name,
            Slug = sanctuarySideDTO.Slug,
            Regions = null,
            Church = sanctuarySideChurch,
        };

        if (sanctuarySideChurch != null) {
            sanctuarySideChurch.Sides.Add(sanctuarySide);
        }

        return sanctuarySide;
    }
}