using StainedGlass.Persistence.Entities;
using StainedGlass.Persistence.Transfer;
using StainedGlass.Transfer.Mapper;

namespace StainedGlass.Persistence.Services.Entities;

public class EntityItemType : INonRelatable, IPersistenceService
{
    public void AddEntity(IPersistanceTransferStruct transferStruct)
    {
        var itemTypeDto = (ItemTypeDTO)transferStruct;
        var entity = GetEntity(itemTypeDto) as ItemType;
        EntitiesCollection.ItemsTypes.TryAdd(itemTypeDto.Slug, entity);
    }

    public IEnumerable<IPersistanceTransferStruct> GetAllDtos()
    {
        return EntitiesCollection.ItemsTypes.Select(e => GetDTOForEntity(e.Value)).ToList();
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
        if (EntitiesCollection.ItemsTypes.ContainsKey(slug))
        {
            EntitiesCollection.ItemsTypes.Remove(slug);
        }
    }

    public void ReplaceEntity(string slug, IPersistanceTransferStruct transferStruct)
    {
        if (!EntitiesCollection.ItemsTypes.ContainsKey(slug))
        {
            return;
        }

        var entity = GetEntity(transferStruct);
        entity.Slug = slug;
        EntitiesCollection.ItemsTypes[slug] = (ItemType)entity;
    }

    public IPersistanceTransferStruct? GetDTOForEntity(
        IEntity? entity, 
        bool skipParentElements = false, 
        bool skipChildrenElements = false
        )
    {
        var ItemTypeEntity = entity as ItemType;
        
        return new ItemTypeDTO
        {
            Name = ItemTypeEntity.Name,
            Slug = ItemTypeEntity.Slug,
        };
    }

    public IEntity GetEntity(IPersistanceTransferStruct transferable)
    {
        ItemTypeDTO? itemTypeDTO = transferable as ItemTypeDTO?;
        
        if (itemTypeDTO == null)
            return null;
        
        return new ItemType
        {
            Name = itemTypeDTO.Value.Name,
            Slug = itemTypeDTO.Value.Slug,
        };
    }
}