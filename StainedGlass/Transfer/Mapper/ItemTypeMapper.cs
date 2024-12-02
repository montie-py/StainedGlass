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
    
    public override Transferable? GetDTOBySlug(string slug)
    {
        var nullableTransferItemTypeDto = _persistenceService.GetDto(slug);
        if (nullableTransferItemTypeDto is null)
        {
            return null;
        }

        var transferItemTypeDto = (Persistence.Transfer.ItemTypeDTO)nullableTransferItemTypeDto;

        return new ItemTypeDTO
        {
            Name = transferItemTypeDto.Name,
            Slug = transferItemTypeDto.Slug,
        };
    }
    
    public override IEnumerable<Transferable?> GetAllDTOs()
    {
        var transferItemTypeDtos = _persistenceService.GetAllDtos() as IEnumerable<Persistence.Transfer.ItemTypeDTO>;
        var itemTypeDtos = new List<Transferable?>();
        foreach (var transferItemTypeDto in transferItemTypeDtos)
        {
            var itemTypeDto = new ItemTypeDTO
            {
                Name = transferItemTypeDto.Name,
                Slug = transferItemTypeDto.Slug,
            };
            
            itemTypeDtos.Add(itemTypeDto);
        }

        return itemTypeDtos;
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

    public void RemoveEntity(string slug)
    {
        if (EntitiesCollection.ItemsTypes.ContainsKey(slug))
        {
            EntitiesCollection.ItemsTypes[slug].Remove();
        }
    }
}