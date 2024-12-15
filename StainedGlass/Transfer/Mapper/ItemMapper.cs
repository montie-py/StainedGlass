
using StainedGlass.Persistence.Templates;
using StainedGlass.Persistence.Transfer;
using ItemDTO = StainedGlass.Transfer.DTOs.ItemDTO;
using ItemTypeDTO = StainedGlass.Transfer.DTOs.ItemTypeDTO;
using SanctuaryRegionDTO = StainedGlass.Transfer.DTOs.SanctuaryRegionDTO;

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

    public override void ReplaceEntity(string slug, Transferable transferable)
    {
        Persistence.Transfer.ItemDTO transferItemDTO = transferable as ItemDTO;
        _persistenceService.ReplaceEntity(slug, transferItemDTO);
    }

    public override Transferable? GetDTOBySlug(string slug)
    {
        var nullableTransferItemDto = _persistenceService.GetDtoBySlug(slug);
        if (nullableTransferItemDto is null)
        {
            return null;
        }

        return GetDtoFromTransferable(nullableTransferItemDto);
    }
    
    public override ICollection<Transferable?> GetAllDTOs()
    {
        var transferItemDtos = _persistenceService.GetAllDtos();
        var itemDtos = new List<Transferable>();
        foreach (IPersistanceTransferStruct transferItemDto in transferItemDtos)
        {
            itemDtos.Add(GetDtoFromTransferable(transferItemDto));
        }
        return itemDtos;
    }
    
    public override void RemoveEntity(string slug)
    {
        _persistenceService.RemoveEntity(slug);
    }
    
    protected override Transferable GetDtoFromTransferable(IPersistanceTransferStruct transferStruct)
    {
        var transferItemDto = (Persistence.Transfer.ItemDTO)transferStruct;

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
                Description = relatedItem.Description,
                Image = relatedItem.Image,
            });
        }
        
        //setting sanctuaryRegion

        SanctuaryRegionDTO sanctuaryRegionDto = new();

        if (transferItemDto.SanctuaryRegion != null)
        {
            var sanctuaryRegionTransfer = (Persistence.Transfer.SanctuaryRegionDTO)transferItemDto.SanctuaryRegion;
            sanctuaryRegionDto = new SanctuaryRegionDTO
            {
                Name = sanctuaryRegionTransfer.Name,
                Slug = sanctuaryRegionTransfer.Slug,
                Image = sanctuaryRegionTransfer.Image,
                Description = sanctuaryRegionTransfer.Description,
            };
        }

        return new ItemDTO
        {
            Title = transferItemDto.Title,
            Slug = transferItemDto.Slug,
            Image = transferItemDto.Image,
            Description = transferItemDto.Description,
            ItemType = itemType,
            ItemTypeSlug = transferItemDto.ItemTypeSlug,
            RelatedItems = relatedItems,
            SanctuaryRegion = sanctuaryRegionDto,
        };
    }
}