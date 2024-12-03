using StainedGlass.Transfer.Mapper;

namespace StainedGlass.Persistence.Services.Entities;

public class ItemType : NonRelatable
{
    public IEnumerable<Transferable?> GetAllDTOs()
    {
        return EntitiesCollection.ItemsTypes.Select(e => GetDTO(e.Value)).ToList();
    }
    
    public Transferable? GetDTOBySlug(string slug)
    {
        if (!EntitiesCollection.ItemsTypes.ContainsKey(slug))
        {
            return null;
        }
        return GetDTO(EntitiesCollection.ItemsTypes[slug]);
    }
    
    public void RemoveEntity(string slug)
    {
        if (EntitiesCollection.ItemsTypes.ContainsKey(slug))
        {
            EntitiesCollection.ItemsTypes[slug].Remove();
        }
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

    public Entity GetEntity(Transferable transferable)
    {
        ItemTypeDTO itemTypeDTO = transferable as ItemTypeDTO;
        return new ItemType
        {
            Name = itemTypeDTO.Name,
            Slug = itemTypeDTO.Slug,
        };
    }
}