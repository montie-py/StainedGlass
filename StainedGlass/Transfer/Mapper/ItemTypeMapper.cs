using StainedGlass.Persistence.Templates;
using StainedGlass.Persistence.Transfer;
using ItemTypeDTO = StainedGlass.Transfer.DTOs.ItemTypeDTO;
using ItemDTO = StainedGlass.Transfer.DTOs.ItemDTO;

namespace StainedGlass.Transfer.Mapper;

internal class ItemTypeMapper : Mapper
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

    public override void ReplaceEntity(string slug, Transferable transferable)
    {
        Persistence.Transfer.ItemTypeDTO transferItemTypeDto = transferable as ItemTypeDTO;
        _persistenceService.ReplaceEntity(slug, transferItemTypeDto);
    }

    public override Transferable? GetDTOBySlug(string slug)
    {
        var nullableTransferItemTypeDto = _persistenceService.GetDtoBySlug(slug);
        if (nullableTransferItemTypeDto is null)
        {
            return null;
        }

        return GetDtoFromTransferable(nullableTransferItemTypeDto);
    }
    
    public override ICollection<Transferable?> GetAllDTOs()
    {
        var transferItemTypeDtos = _persistenceService.GetAllDtos();
        var itemTypeDtos = new List<Transferable?>();
        foreach (Persistence.Transfer.ItemTypeDTO transferItemTypeDto in transferItemTypeDtos)
        {
            itemTypeDtos.Add(GetDtoFromTransferable(transferItemTypeDto));
        }

        return itemTypeDtos;
    }
    
    public override void RemoveEntity(string slug)
    {
        _persistenceService.RemoveEntity(slug);
    }
    
    protected override Transferable GetDtoFromTransferable(IPersistanceTransferStruct transferStruct)
    {
        var transferItemTypeDto = (Persistence.Transfer.ItemTypeDTO)transferStruct;
        
        ICollection<ItemDTO> itemDtos = new List<ItemDTO>();

        if (transferItemTypeDto.Items != null)
        {
            foreach (var itemDto in transferItemTypeDto.Items)
            {
                var item = new ItemDTO
                {
                    Title = itemDto.Title,
                    Description = itemDto.Description,
                    Image = itemDto.Image,
                    Slug = itemDto.Slug,
                };
                itemDtos.Add(item);
            }
        }
        
        return new ItemTypeDTO
        {
            Name = transferItemTypeDto.Name,
            Slug = transferItemTypeDto.Slug,
            Items = itemDtos
        };
    }
}