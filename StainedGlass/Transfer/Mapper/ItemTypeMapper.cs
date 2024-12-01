using StainedGlass.Persistence.Templates;
using StainedGlass.Transfer.DTOs;

namespace StainedGlass.Transfer.Mapper;

internal class ItemTypeMapper : Mapper, NonRelatable
{
    public override void SetInstance(IPersistenceTemplate template)
    {
        _persistenceService = template.GetItemTypeInstance();
    }
    
    public override void SaveEntity(Transferable transferable)
    {
        Persistence.Transfer.ItemTypeDTO transferItemTypeDto = transferable as ItemTypeDTO;
        _persistenceService.AddEntity(transferItemTypeDto);
    }
    public Transferable? GetDTO(Entity? entity, bool skipParentElements = false, bool skipChildrenElements = false)
    {
        var ItemTypeEntity = entity as ItemType;
        
        return new ItemTypeDTO
        {
            Name = ItemTypeEntity.Name,
            Slug = ItemTypeEntity.Slug,
        };
    }

    public Transferable? GetDTOBySlug(string slug)
    {
        if (!EntitiesCollection.ItemsTypes.ContainsKey(slug))
        {
            return null;
        }
        return GetDTO(EntitiesCollection.ItemsTypes[slug]);
    }

    public IEnumerable<Transferable?> GetAllDTOs()
    {
        return EntitiesCollection.ItemsTypes.Select(e => GetDTO(e.Value)).ToList();
    }

    public Entity GetEntity(Transferable transferable)
    {
        ItemTypeDTO itemTypeDTO = transferable as ItemTypeDTO;
        return new ItemType
        {
            Name = itemTypeDTO.Name,
            Slug = itemTypeDTO.Slug,
        };
    }

    public void RemoveEntity(string slug)
    {
        if (EntitiesCollection.ItemsTypes.ContainsKey(slug))
        {
            EntitiesCollection.ItemsTypes[slug].Remove();
        }
    }
}