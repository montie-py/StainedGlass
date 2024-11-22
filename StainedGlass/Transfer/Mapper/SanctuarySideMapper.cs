using StainedGlass.Entities;
using StainedGlass.Entities.Transfer;
using StainedGlass.Transfer.DTOs;

namespace StainedGlass.Transfer.Mapper;

internal class SanctuarySideMapper : NonRelatable
{    
    public Transferable? GetDTO(Entity? entity, bool skipParentElements = false, bool skipChildrenElements = false)
    {
        if (entity == null) 
        {
            return null;
        }
        
        ChurchMapper churchMapper = new();
        SanctuaryRegionMapper sanctuaryRegionMapper = new();
        SanctuarySide? sanctuarySide = entity as SanctuarySide;

        List<SanctuaryRegionDTO> regionsDTO = new();
        ChurchDTO churchDTO = null;
        
        if (!skipChildrenElements)
        {
            foreach (SanctuaryRegion? region in sanctuarySide.Regions)
            {
                regionsDTO.Add(sanctuaryRegionMapper.GetDTO(
                    region, skipParentElements: true, skipChildrenElements: true
                ) as SanctuaryRegionDTO);
            }
        }

        var sanctuarySideDTO = new SanctuarySideDTO
        {
            Slug = sanctuarySide.Slug,
            Name = sanctuarySide.Name,
            Regions = regionsDTO,
            Church = churchDTO
        };

        if (!skipParentElements)
        {
            churchDTO = churchMapper.GetDTO(sanctuarySide.Church) as ChurchDTO;
            if (churchDTO != null)
            {
                sanctuarySideDTO.ChurchSlug = churchDTO.Slug;
            }
        }
        
        return sanctuarySideDTO;
    }

    public Transferable? GetDTOBySlug(string slug)
    {
        if (!EntitiesCollection.SanctuarySides.ContainsKey(slug))
        {
            return null;
        }
        return GetDTO(EntitiesCollection.SanctuarySides[slug], skipParentElements: true);
    }

    public IEnumerable<Transferable?> GetAllDTOs()
    {
        return EntitiesCollection.SanctuarySides.Select(
            e => GetDTO(e.Value, skipParentElements: true)
            );
    }

    public Entity GetEntity(Transferable transferable)
    {
        SanctuarySideDTO sanctuarySideDTO = transferable as SanctuarySideDTO;

        Church sanctuarySideChurch = 
            (
                sanctuarySideDTO.ChurchSlug != null 
                && EntitiesCollection.Churches.ContainsKey(sanctuarySideDTO.ChurchSlug)
                ) ? EntitiesCollection.Churches[sanctuarySideDTO.ChurchSlug] : null;

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

    public void RemoveEntity(string slug)
    {
        EntitiesCollection.SanctuarySides.Remove(slug);
    }
}