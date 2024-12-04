using StainedGlass.Persistence.Transfer;
using StainedGlass.Transfer.Mapper;

namespace StainedGlass.Persistence.Services.Entities;

public class EntitySanctuaryRegion : INonRelatable, IPersistenceService
{
    public IEnumerable<Transferable?> GetAllDTOs()
    {
        return EntitiesCollection.SanctuaryRegions.Select(
            e => GetDTO(e.Value, skipParentElements: true)
        );
    }
    
    public Transferable? GetDTOBySlug(string slug)
    {
        if (!EntitiesCollection.SanctuaryRegions.ContainsKey(slug))
        {
            return null;
        }
        return GetDTO(EntitiesCollection.SanctuaryRegions[slug], skipParentElements: true);
    }

    public void AddEntity(IPersistanceTransferStruct transferStruct)
    {
        EntitiesCollection.SanctuaryRegions.TryAdd(Slug, this);
    }

    public IEnumerable<IPersistanceTransferStruct> GetAllDtos()
    {
        throw new NotImplementedException();
    }

    public IPersistanceTransferStruct? GetDto(string slug)
    {
        throw new NotImplementedException();
    }

    public void RemoveEntity(string slug)
    {
        if (!EntitiesCollection.SanctuaryRegions.ContainsKey(slug))
        {
            return;
        }
        
        //remove region from its side
        var sanctuarySide = EntitiesCollection.SanctuarySides.Values.FirstOrDefault(e => 
            e.Regions?.FirstOrDefault(r => r.Slug == Slug) != null
        );
        if (sanctuarySide != null)
        {
            sanctuarySide.Regions?.RemoveAll(e => e.Slug == Slug);
        }
        
        EntitiesCollection.SanctuaryRegions.Remove(Slug);
    }

    public void ReplaceEntity(string slug, IPersistanceTransferStruct transferStruct)
    {
        if (!EntitiesCollection.SanctuaryRegions.ContainsKey(slug))
        {
            return;
        }
        entity.Slug = slug;
        var oldEntity = EntitiesCollection.SanctuaryRegions[slug];
        EntitiesCollection.SanctuaryRegions[slug] = (SanctuaryRegion)entity;
        //if old entity has an assigned side - new one cannot lack one
        if (EntitiesCollection.SanctuaryRegions[slug].SanctuarySide is null)
        {
            EntitiesCollection.SanctuaryRegions[slug].SanctuarySide = oldEntity.SanctuarySide;
        }
        EntitiesCollection.SanctuaryRegions[slug].Items = oldEntity.Items;
    }

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
}