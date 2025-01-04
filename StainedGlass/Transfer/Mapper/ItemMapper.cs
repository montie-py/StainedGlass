
using StainedGlass.Persistence.Templates;
using StainedGlass.Persistence.Transfer;
using ItemDTO = StainedGlass.Transfer.DTOs.ItemDTO;
using ItemTypeDTO = StainedGlass.Transfer.DTOs.ItemTypeDTO;
using ItemImageDTO = StainedGlass.Transfer.DTOs.ItemImageDTO;
using SanctuaryRegionDTO = StainedGlass.Transfer.DTOs.SanctuaryRegionDTO;

namespace StainedGlass.Transfer.Mapper;

internal class  ItemMapper : Mapper
{
    public override void SetInstance(IPersistenceTemplate template)
    {
        _persistenceService = template.GetItemInstance();
    }
    
    public override async Task<bool> SaveEntity(Transferable transferable)
    {
        Persistence.Transfer.ItemDTO transferItemDTO = transferable as ItemDTO;
        return await _persistenceService.AddEntity(transferItemDTO);
    }

    public override async Task<bool> ReplaceEntity(string slug, Transferable transferable)
    {
        Persistence.Transfer.ItemDTO transferItemDTO = transferable as ItemDTO;
        return await _persistenceService.ReplaceEntity(slug, transferItemDTO);
    }

    public override async Task<Transferable?> GetDTOBySlug(string slug)
    {
        var nullableTransferItemDto = await _persistenceService.GetDtoBySlug(slug);
        if (nullableTransferItemDto is null)
        {
            return null;
        }

        return GetDtoFromTransferable(nullableTransferItemDto);
    }
    
    public override async Task<ICollection<Transferable?>> GetAllDTOs()
    {
        var transferItemDtos = await _persistenceService.GetAllDtos();
        var itemDtos = new List<Transferable>();
        foreach (IPersistanceTransferStruct transferItemDto in transferItemDtos)
        {
            itemDtos.Add(GetDtoFromTransferable(transferItemDto));
        }
        return itemDtos;
    }
    
    public override async Task<bool> RemoveEntity(string slug)
    {
        return await _persistenceService.RemoveEntity(slug);
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

        var newItem = new ItemDTO
        {
            Title = transferItemDto.Title,
            Slug = transferItemDto.Slug,
            Description = transferItemDto.Description,
            ItemType = itemType,
            ItemTypeSlug = transferItemDto.ItemTypeSlug,
            RelatedItems = relatedItems,
            SanctuaryRegion = sanctuaryRegionDto,
        };

        foreach (var itemImage in transferItemDto.ItemImages)
        {
            var itemImageDto = new ItemImageDTO
            {
                Image = itemImage.Image,
                Slug = itemImage.Slug,
            };
            
            newItem.ItemImages.Add(itemImageDto);
        }

        return newItem;
    }
}