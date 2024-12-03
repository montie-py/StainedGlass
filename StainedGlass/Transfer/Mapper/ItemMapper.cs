
using StainedGlass.Persistence.Services.Entities;
using StainedGlass.Persistence.Templates;
using StainedGlass.Transfer.DTOs;

namespace StainedGlass.Transfer.Mapper;

internal class ItemMapper : Mapper
{
    public override void SetInstance(IPersistenceTemplate template)
    {
        _persistenceService = template.GetItemInstance();
    }
    
    public override void SaveEntity(Transferable transferable)
    {
        Persistence.Transfer.ItemDTO transferItemDTO = transferable as ItemDTO;
        _persistenceService.AddEntity(transferItemDTO);
    }
    
    public override Transferable? GetDTOBySlug(string slug)
    {
        var nullableTransferItemDto = _persistenceService.GetDto(slug);
        if (nullableTransferItemDto is null)
        {
            return null;
        }
        
        var transferItemDto = (Persistence.Transfer.ItemDTO)nullableTransferItemDto;

        //setting itemType
        var itemType = new ItemTypeDTO
        {
            Name = transferItemDto.ItemType.Name,
            Slug = transferItemDto.ItemType.Slug,
        };
        
        //setting relatedItems
        var relatedItems = new Dictionary<string, ItemDTO>();
        foreach (var relatedItem in transferItemDto.RelatedItems.Values)
        {
            relatedItems.Add(relatedItem.Slug, new ItemDTO
            {
                Title = relatedItem.Title,
                Slug = relatedItem.Slug,
            });
        }

        return new ItemDTO
        {
            Title = transferItemDto.Title,
            Slug = transferItemDto.Slug,
            Image = transferItemDto.Image,
            Description = transferItemDto.Description,
            ItemType = itemType,
            RelatedItems = relatedItems
        };
    }
    
    public override IEnumerable<Transferable?> GetAllDTOs()
    {
        var transferItemDtos = _persistenceService.GetAllDtos() as IEnumerable<Persistence.Transfer.ItemDTO>;
        var itemDtos = new List<ItemDTO>();
        foreach (var transferItemDto in transferItemDtos)
        {
            //setting itemtype
            var itemType = new ItemTypeDTO
            {
                Name = transferItemDto.ItemType.Name,
                Slug = transferItemDto.ItemType.Slug,
            };
            
            //setting relateditems
            var relatedItems = new Dictionary<string, ItemDTO>();
            foreach (var relatedItem in transferItemDto.RelatedItems.Values)
            {
                relatedItems.Add(relatedItem.Slug, new ItemDTO
                {
                    Title = relatedItem.Title,
                    Slug = relatedItem.Slug,
                });
            }
            
            var itemDto = new ItemDTO
            {
                Title = transferItemDto.Title,
                Slug = transferItemDto.Slug,
                Description = transferItemDto.Description,
                Image = transferItemDto.Image,
                ItemType = itemType,
                RelatedItems = relatedItems
            };
            itemDtos.Add(itemDto);
        }
        return itemDtos;
    }
    
    public override void RemoveEntity(string slug)
    {
        _persistenceService.RemoveEntity(slug);
    }
}