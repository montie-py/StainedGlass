using StainedGlass.Entities;
using StainedGlass.Entities.Transfer;
using StainedGlass.Transfer.DTOs;

namespace StainedGlass.Transfer.Mapper;

public class ItemTypeMapper : NonRelatable
{
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
        EntitiesCollection.ItemsTypes.Remove(slug);
    }
}