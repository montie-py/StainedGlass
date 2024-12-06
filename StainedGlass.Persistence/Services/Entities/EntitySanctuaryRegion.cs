using StainedGlass.Persistence.Entities;
using StainedGlass.Persistence.Transfer;
using StainedGlass.Transfer.Mapper;

namespace StainedGlass.Persistence.Services.Entities;

public class EntitySanctuaryRegion : INonRelatable, IPersistenceService
{
    public void AddEntity(IPersistanceTransferStruct transferStruct)
    {
        var sanctuaryRegionDto = (SanctuaryRegionDTO)transferStruct;
        var entity = GetEntity(sanctuaryRegionDto) as SanctuaryRegion;
        
        EntitiesCollection.SanctuaryRegions.TryAdd(sanctuaryRegionDto.Slug, entity);
    }
    
    public IEnumerable<IPersistanceTransferStruct> GetAllDtos()
    {
        return EntitiesCollection.SanctuaryRegions.Select(
            e => GetDTOForEntity(e.Value, skipParentElements: true)
        );
    }
    
    public IPersistanceTransferStruct? GetDTOForEntity(
        IEntity? entity, 
        bool skipParentElements = false, 
        bool skipChildrenElements = false
    )
    {
        if (entity == null) 
        {
            return null;
        }
        EntityItem entityItem = new();
        EntitySanctuarySide entitySanctuarySide = new();

        SanctuaryRegion region = entity as SanctuaryRegion;
        HashSet<ItemDTO> WindowsDTOs = new();
        SanctuarySideDTO? sanctuarySideDTO = null;

        if (!skipChildrenElements)
        {
            foreach (Item window in region.Items)
            {
                WindowsDTOs.Add((ItemDTO)entityItem.GetDTOForEntity(window, skipParentElements: true));
            }
        }

        if (!skipParentElements)
        {
            sanctuarySideDTO = (SanctuarySideDTO)entitySanctuarySide.GetDTOForEntity(
                region.SanctuarySide, skipParentElements: true, skipChildrenElements: true
            );
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
            sanctuaryRegionDTO.SanctuarySideSlug = sanctuarySideDTO.Value.Slug;
        }

        return sanctuaryRegionDTO;
    }

    public IPersistanceTransferStruct? GetDtoBySlug(string slug)
    {
        if (!EntitiesCollection.ItemsTypes.ContainsKey(slug))
        {
            return null;
        }
        return GetDTOForEntity(EntitiesCollection.ItemsTypes[slug]);
    }

    public void RemoveEntity(string slug)
    {
        if (!EntitiesCollection.SanctuaryRegions.ContainsKey(slug))
        {
            return;
        }
        
        //remove region from its side
        var sanctuarySide = EntitiesCollection.SanctuarySides.Values.FirstOrDefault(e => 
            e.Regions?.FirstOrDefault(r => r.Slug == slug) != null
        );
        if (sanctuarySide != null)
        {
            ((List<SanctuaryRegion>)sanctuarySide.Regions)?.RemoveAll(e => e.Slug == slug);
        }
        
        EntitiesCollection.SanctuaryRegions.Remove(slug);
    }

    public void ReplaceEntity(string slug, IPersistanceTransferStruct transferStruct)
    {
        if (!EntitiesCollection.SanctuaryRegions.ContainsKey(slug))
        {
            return;
        }

        var entity = GetEntity(transferStruct);
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

    public IEntity GetEntity(IPersistanceTransferStruct transferable)
     {
        SanctuaryRegionDTO? sanctuaryRegionDTO = transferable as SanctuaryRegionDTO?;

        if (sanctuaryRegionDTO == null)
        {
            return null;
        }

        SanctuarySide sanctuarySide = 
            (
                EntitiesCollection.SanctuarySides.ContainsKey(sanctuaryRegionDTO.Value.SanctuarySideSlug)
                ) ? EntitiesCollection.SanctuarySides[sanctuaryRegionDTO.Value.SanctuarySideSlug] : null;
        
        SanctuaryRegion sanctuaryRegion = new SanctuaryRegion
        {
            Name = sanctuaryRegionDTO.Value.Name,
            Description = sanctuaryRegionDTO.Value.Description,
            Slug = sanctuaryRegionDTO.Value.Slug,
            Image = sanctuaryRegionDTO.Value.Image,
            Items = null,
            SanctuarySide = sanctuarySide,
        };
        
        if (sanctuaryRegionDTO.Value.Items != null)
        {
            var entityItem = new EntityItem();
            sanctuaryRegion.Items = sanctuaryRegionDTO.Value.Items.Select(
                e => entityItem.GetEntity(e) as Item
                ).ToHashSet();
        }

        if (sanctuarySide != null)
        {
            sanctuarySide.Regions.Add(sanctuaryRegion);
        }

        return sanctuaryRegion;
     }
}