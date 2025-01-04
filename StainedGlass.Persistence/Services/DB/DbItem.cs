using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using StainedGlass.Persistence.Entities;
using StainedGlass.Persistence.Transfer;

namespace StainedGlass.Persistence.Services.DB;

internal class DbItem : DatabasePersistenceService
{
    public override async Task<bool> AddEntity(IPersistanceTransferStruct transferStruct)
    {
        var itemStruct = (ItemDTO)GetDtoFromTransfer(transferStruct);
        var newItem = new Item
        {
            Title = itemStruct.Title,
            Slug = itemStruct.Slug,
            Description = itemStruct.Description,
            SanctuaryRegionSlug = itemStruct.SanctuaryRegionSlug,
            ItemTypeSlug = itemStruct.ItemTypeSlug
        };

        foreach (var itemImageDto in itemStruct.ItemImages)
        {
            var itemImage = new ItemImage
            {
                Image = await FormFileToBytes(itemImageDto.Image),
                Item = newItem,
                Slug = itemImageDto.Slug
            };
            newItem.ItemImages.Add(itemImage);
        }
        
        _dbContext.Items.Add(newItem);
        await _dbContext.SaveChangesAsync();

        if (itemStruct.RelatedItemsSlugs != null)
        {
            foreach (var relatedItemsSlug in itemStruct.RelatedItemsSlugs)
            {
                AddRelatedItem(newItem.Slug, relatedItemsSlug);
            }
            await _dbContext.SaveChangesAsync();
        }
        return true;
    }

    public override async Task<ICollection<IPersistanceTransferStruct>> GetAllDtos()
    {
        List<IPersistanceTransferStruct> itemDtos = new List<IPersistanceTransferStruct>();
        
        foreach (var dbItem in await _dbContext.Items
                     .Include(i => i.ItemType)
                     .Include(i => i.SanctuaryRegion)
                     .Include(i => i.RelatedItems)
                     .ThenInclude(ir => ir.RelatedItem)
                     .ToListAsync())
        {
            itemDtos.Add(GetDtoFromEntity(dbItem));
        }
        return itemDtos;
    }

    public override async Task<IPersistanceTransferStruct?> GetDtoBySlug(string slug)
    {
        var entity = await _dbContext.Items
            .Include(i => i.ItemType)
            .Include(i => i.SanctuaryRegion)
            .Include(i => i.RelatedItems)
            .ThenInclude(ir => ir.RelatedItem)
            .FirstOrDefaultAsync(x => x.Slug == slug);
        if (entity is null)
        {
            return null;
        }

        return GetDtoFromEntity(entity);
    }
    
    public override async Task<bool> ReplaceEntity(string slug, IPersistanceTransferStruct transferStruct)
    {
        var itemEntity = await _dbContext.Items
            .Include(x => x.RelatedItems)
            .FirstOrDefaultAsync(x => x.Slug == slug);
        if (itemEntity != null)
        {
            var transferItemDto = (ItemDTO)GetDtoFromTransfer(transferStruct);
            itemEntity.Title = transferItemDto.Title;
            itemEntity.Description = transferItemDto.Description;
            itemEntity.ItemTypeSlug = transferItemDto.ItemTypeSlug;
            itemEntity.SanctuaryRegionSlug = itemEntity.SanctuaryRegionSlug;
            
            //handle itemimages
            if (transferItemDto.ItemImages.Count > 0)
            {
                foreach (var itemImageDto in transferItemDto.ItemImages)
                {
                    var itemImage = new ItemImage
                    {
                        Image = await FormFileToBytes(itemImageDto.Image),
                        Item = itemEntity,
                        Slug = itemImageDto.Slug
                    };
                    itemEntity.ItemImages.Add(itemImage);
                }
            }
            
            await _dbContext.SaveChangesAsync();

            if (transferItemDto.RelatedItemsSlugs != null)
            {
                //if this item has any related items at all
                if (itemEntity.RelatedItems.Count > 0)
                {
                    foreach (var relatedItemSlug in transferItemDto.RelatedItemsSlugs)
                    {
                        //if there is no relateditem with such a slug - add one
                        var existingRelatedItem = itemEntity.RelatedItems.FirstOrDefault(
                            x => x.ItemSlug == relatedItemSlug || x.RelatedItemSlug == relatedItemSlug
                            );
                        if (existingRelatedItem is null)
                        {
                            AddRelatedItem(itemEntity.Slug, relatedItemSlug);
                        }
                    }
                    await _dbContext.SaveChangesAsync();
                }
                else
                //if not - add new relations
                {
                    foreach (var relatedItemSlug in transferItemDto.RelatedItemsSlugs)
                    {
                        AddRelatedItem(itemEntity.Slug, relatedItemSlug);
                    }
                    await _dbContext.SaveChangesAsync();
                }
                
            }
        }

        return true;
    }

    public override async Task<bool> RemoveEntity(string slug)
    {
        var item = await _dbContext.Items.FirstOrDefaultAsync(x => x.Slug == slug);
        if (item != null)
        {
            //removing itemRelation first
            var list = _dbContext.ItemRelations
                .Where(x => x.ItemSlug == slug || x.RelatedItemSlug == slug)
                .ToList();
            _dbContext.RemoveRange(list);
            
            _dbContext.Items.Remove(item);
            await _dbContext.SaveChangesAsync();
        }

        return true;
    }

    private List<ItemRelation> GetRelatedItemsBySlug(string slug)
    {
        using (var context = new AppDbContext())
        {
            return context.Items
                .Include(e => e.RelatedItems)
                .FirstOrDefault(e => e.Slug == slug)!.RelatedItems.ToList();
        }
    }
    
    protected override IPersistanceTransferStruct GetDtoFromTransfer(IPersistanceTransferStruct transferStruct)
    {
        return (ItemDTO)transferStruct;
    }

    protected override IPersistanceTransferStruct GetDtoFromEntity(IEntity entity)
    {
        var itemEntity = (Item)entity;
        //adding related items
        var relatedItems = new Dictionary<string, ItemDTO>();
        SanctuaryRegionDTO sanctuaryRegionDto = new();

        if (itemEntity.RelatedItems != null)
        {
            foreach (var relatedItem in itemEntity.RelatedItems)
            {
                relatedItems.Add(
                    relatedItem.RelatedItem.Slug, 
                    new ItemDTO
                    {
                        Title = relatedItem.RelatedItem.Title,
                        Slug = relatedItem.RelatedItem.Slug,
                        Description = relatedItem.RelatedItem.Description,
                    }
                );
            }
        }

        //adding sanctuaryRegion
        if (itemEntity.SanctuaryRegion != null)
        {
            sanctuaryRegionDto = new SanctuaryRegionDTO
            {
                Slug = itemEntity.SanctuaryRegion.Slug,
                Name = itemEntity.SanctuaryRegion.Name,
                // Image = itemEntity.SanctuaryRegion.Image,
                Description = itemEntity.SanctuaryRegion.Description,
            };
        }
        
        //adding itemType
        var itemTypeDto = new ItemTypeDTO
        {
            Name = itemEntity.ItemType.Name,
            Slug = itemEntity.ItemType.Slug,
        };

        var newItemDto = new ItemDTO
        {
            Title = itemEntity.Title,
            Slug = itemEntity.Slug,
            Description = itemEntity.Description,
            RelatedItems = relatedItems,
            ItemType = itemTypeDto,
            ItemTypeSlug = itemEntity.ItemTypeSlug,
            SanctuaryRegion = sanctuaryRegionDto,
            SanctuaryRegionSlug = itemEntity.SanctuaryRegionSlug
        };

        foreach (var itemImage in itemEntity.ItemImages)
        {
            var itemImageDto = new ItemImageDTO
            {
                Image = new FormFile(itemImage.Image, FileName, fileType),
                Slug = itemImage.Slug,
                ItemSlug = newItemDto.Slug
            };
            newItemDto.ItemImages.Add(itemImageDto);
        }
  
        return newItemDto;
    }
    
    private void AddRelatedItem(string itemSlug, string relatedItemSlug)
    {
        var itemRelation = new ItemRelation
        {
            ItemSlug = itemSlug,
            RelatedItemSlug = relatedItemSlug,
        };
        _dbContext.ItemRelations.Add(itemRelation);
        itemRelation = new ItemRelation
        {
            ItemSlug = relatedItemSlug,
            RelatedItemSlug = itemSlug,
        };
        _dbContext.ItemRelations.Add(itemRelation);
    }
    
    private async Task<byte[]> FormFileToBytes(IFormFile image)
    {
        //transferring file from IFormFile to byte[]
        byte[] filebytes;
        using (var ms = new MemoryStream())
        {
            await image.CopyToAsync(ms);
            filebytes = ms.ToArray();
        }

        return filebytes;
    }
}