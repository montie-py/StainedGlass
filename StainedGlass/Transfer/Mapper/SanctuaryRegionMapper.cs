using StainedGlass.Entities;
using StainedGlass.Entities.Transfer;
using StainedGlass.Transfer.DTOs;

namespace StainedGlass.Transfer.Mapper;

internal class SanctuaryRegionMapper : NonRelatable
{
    
    public Transferable? GetDTO(Entity? entity, bool skipParentElements = false, bool skipChildrenElements = false)
     {
        if (entity == null) 
        {
            return null;
        }
        ItemMapper itemMapper = new();
        SanctuarySideMapper sanctuarySideMapper = new();

        SanctuaryRegion region = entity as SanctuaryRegion;
        HashSet<ItemDTO> WindowsDTOs = new();
        SanctuarySideDTO sanctuarySideDTO = null;

        if (!skipChildrenElements)
        {
            foreach (Item window in region.Items)
            {
                WindowsDTOs.Add(itemMapper.GetDTO(window, skipParentElements: true) as ItemDTO);
            }
        }

        if (!skipParentElements)
        {
            sanctuarySideDTO = sanctuarySideMapper.GetDTO(
                region.SanctuarySide, skipParentElements: true, skipChildrenElements: true
                ) as SanctuarySideDTO;
        }

        var sanctuaryRegionDTO = new SanctuaryRegionDTO
        {
            Name = region.Name,
            Description = region.Description,
            Slug = region.Slug,
            Image = region.Image,
            Items = WindowsDTOs,
            SanctuarySide = sanctuarySideDTO,
        };

        if (sanctuarySideDTO != null)
        {
            sanctuaryRegionDTO.SanctuarySideSlug = sanctuarySideDTO.Slug;
        }

        return sanctuaryRegionDTO;
     }

    public Transferable? GetDTOBySlug(string slug)
    {
        if (!EntitiesCollection.SanctuaryRegions.ContainsKey(slug))
        {
            return null;
        }
        return GetDTO(EntitiesCollection.SanctuaryRegions[slug], skipParentElements: true);
    }

    public IEnumerable<Transferable?> GetAllDTOs()
    {
        return EntitiesCollection.SanctuaryRegions.Select(
            e => GetDTO(e.Value, skipParentElements: true)
            );
    }

    public Entity GetEntity(Transferable transferable)
     {
        SanctuaryRegionDTO sanctuaryRegionDTO = transferable as SanctuaryRegionDTO;

        SanctuarySide sanctuarySide = 
            (
                sanctuaryRegionDTO.SanctuarySideSlug != null 
                && EntitiesCollection.SanctuarySides.ContainsKey(sanctuaryRegionDTO.SanctuarySideSlug)
                ) ? EntitiesCollection.SanctuarySides[sanctuaryRegionDTO.SanctuarySideSlug] : null;
        
        SanctuaryRegion sanctuaryRegion = new SanctuaryRegion
        {
            Name = sanctuaryRegionDTO.Name,
            Description = sanctuaryRegionDTO.Description,
            Slug = sanctuaryRegionDTO.Slug,
            Image = sanctuaryRegionDTO.Image,
            Items = null,
            SanctuarySide = sanctuarySide,
        };
        
        if (sanctuaryRegionDTO.Items != null)
        {
            var stainedGlassMapper = new ItemMapper();
            HashSet<Entities.Item> windows = new();
            sanctuaryRegion.Items = sanctuaryRegionDTO.Items.Select(
                e => stainedGlassMapper.GetEntity(e) as Entities.Item
                ).ToHashSet();
        }

        if (sanctuarySide != null)
        {
            sanctuarySide.Regions.Add(sanctuaryRegion);
        }

        return sanctuaryRegion;
     }

    public void RemoveEntity(string slug)
    {
        EntitiesCollection.SanctuaryRegions[slug].Remove();
    }
}