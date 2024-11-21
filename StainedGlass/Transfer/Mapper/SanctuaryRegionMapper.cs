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
            foreach (Item window in region.Windows)
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
            Description = region.Name,
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

    public Transferable? GetDTOBySlug(string slug)
    {
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
            EntitiesCollection.SanctuarySides.ContainsKey(sanctuaryRegionDTO.Slug) ?
                EntitiesCollection.SanctuarySides[sanctuaryRegionDTO.Slug] : null;
        
        SanctuaryRegion sanctuaryRegion = new SanctuaryRegion
        {
            Name = sanctuaryRegionDTO.Name,
            Description = sanctuaryRegionDTO.Description,
            Slug = sanctuaryRegionDTO.Slug,
            Image = sanctuaryRegionDTO.Image,
            Windows = null,
            SanctuarySide = sanctuarySide,
        };
        
        if (sanctuaryRegionDTO.Windows != null)
        {
            var stainedGlassMapper = new ItemMapper();
            HashSet<Entities.Item> windows = new();
            // sanctuaryRegion.Windows = (HashSet<Entities.StainedGlass>)sanctuaryRegionDTO.Windows.Select(
            //     e => (Entities.StainedGlass)stainedGlassMapper.GetEntity(e)
            //     );
            sanctuaryRegion.Windows = sanctuaryRegionDTO.Windows.Select(
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
        EntitiesCollection.SanctuaryRegions.Remove(slug);
    }
}