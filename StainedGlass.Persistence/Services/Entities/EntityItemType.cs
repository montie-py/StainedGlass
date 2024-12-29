using StainedGlass.Persistence.Entities;
using StainedGlass.Persistence.Transfer;
using StainedGlass.Transfer.Mapper;

namespace StainedGlass.Persistence.Services.Entities;

public class EntityItemType : INonRelatable, IPersistenceService
{
    public async Task<bool> AddEntity(IPersistanceTransferStruct transferStruct)
    {
        var itemTypeDto = (ItemTypeDTO)transferStruct;
        var entity = GetEntity(itemTypeDto) as ItemType;
        EntitiesCollection.ItemsTypes.TryAdd(itemTypeDto.Slug, entity);
        return true;
    }

    public async Task<ICollection<IPersistanceTransferStruct>> GetAllDtos()
    {
        return EntitiesCollection.ItemsTypes.Select(e => GetDTOForEntity(e.Value)).ToList();
    }

    public async Task<IPersistanceTransferStruct?> GetDtoBySlug(string slug)
    {
        if (!EntitiesCollection.ItemsTypes.ContainsKey(slug))
        {
            return null;
        }
        return GetDTOForEntity(EntitiesCollection.ItemsTypes[slug]);
    }

    public async Task<bool> RemoveEntity(string slug)
    {
        if (EntitiesCollection.ItemsTypes.ContainsKey(slug))
        {
            EntitiesCollection.ItemsTypes.Remove(slug);
        }

        return true;
    }

    public async Task<bool> ReplaceEntity(string slug, IPersistanceTransferStruct transferStruct)
    {
        if (!EntitiesCollection.ItemsTypes.ContainsKey(slug))
        {
            return false;
        }

        var entity = GetEntity(transferStruct);
        entity.Slug = slug;
        EntitiesCollection.ItemsTypes[slug] = (ItemType)entity;
        return true;
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