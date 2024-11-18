using StainedGlass.Entities;
using StainedGlass.Entities.Transfer;
using StainedGlass.Transfer.DTOs;

namespace StainedGlass.Transfer.Mapper;

public class ItemTypeMapper : NonRelatable
{
    public Transferable? GetDTO(Entity? entity)
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
        return GetDTO(EntitiesCollection.ItemsTypes[slug]);
    }

    public Entity GetEntity(Transferable transferable)
    {
        var itemTypeDTO = transferable as ItemType;
        return new ItemType
        {
            Name = itemTypeDTO.Name,
            Slug = itemTypeDTO.Slug,
        };
    }
}