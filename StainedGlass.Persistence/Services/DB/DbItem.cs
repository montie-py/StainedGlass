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
            Position = itemStruct.Position,
            SanctuaryRegionSlug = itemStruct.SanctuaryRegionSlug,
            ItemTypeSlug = itemStruct.ItemTypeSlug
        };

        Guid guid = Guid.NewGuid();
        foreach (var itemImageFormFile in itemStruct.ItemImages)
        {
            var itemImage = new ItemImage
            {
                Image = await FormFileToBytes(itemImageFormFile),
                Item = newItem,
                Slug = itemImageFormFile.FileName + guid
            };
            newItem.ItemImages.Add(itemImage);
        }
        
        _dbContext.Items.Add(newItem);
        await _dbContext.SaveChangesAsync();

        if (itemStruct.RelatedItemsSlugs != null)
        {
            foreach (var relatedItemsSlug in itemStruct.RelatedItemsSlugs)
            {
                await AddRelatedItem(newItem.Slug, relatedItemsSlug);
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
                     .Include(i => i.ItemImages)
                     .Include(i => i.RelatedItems)
                     .ThenInclude(ir => ir.RelatedItem)
                     .ToListAsync())
        {
            itemDtos.Add(GetDtoFromEntity(dbItem));
        }
        return itemDtos;
    }

    public override async Task<IPersistanceTransferStruct?> GetDtoBySlug(string slug, bool includeChildrenToTheResponse)
    {
        var entity = await _dbContext.Items
            .Include(i => i.ItemType)
            .Include(i => i.SanctuaryRegion)
            .Include(i => i.ItemImages)
            .Include(i => i.RelatedItems)
            .ThenInclude(ir => ir.RelatedItem)
            .FirstOrDefaultAsync(x => x.Slug == slug);
        if (entity is null)
        {
            return null;
        }

        return GetDtoFromEntity(entity);
    }
    
    public override async Task<bool> ReplaceEntity(string itemSlug, IPersistanceTransferStruct transferStruct)
    {
        var itemEntity = await _dbContext.Items
            .Include(x => x.RelatedItems)
            .FirstOrDefaultAsync(x => x.Slug == itemSlug);
        if (itemEntity != null)
        {
            var transferItemDto = (ItemDTO)GetDtoFromTransfer(transferStruct);
            itemEntity.Title = transferItemDto.Title;
            itemEntity.Description = transferItemDto.Description;
            itemEntity.Position = transferItemDto.Position;
            itemEntity.ItemTypeSlug = transferItemDto.ItemTypeSlug;
            itemEntity.SanctuaryRegionSlug = transferItemDto.SanctuaryRegionSlug;
            
            //handle itemimages
            if (transferItemDto.ItemImages.Count > 0)
            {
                foreach (IFormFile itemImageDto in transferItemDto.ItemImages)
                {
                    var itemImage = new ItemImage
                    {
                        Image = await FormFileToBytes(itemImageDto),
                        Item = itemEntity,
                        Slug = itemImageDto.FileName.ToLower()
                    };
                    itemEntity.ItemImages.Add(itemImage);
                }
            }
            
            await _dbContext.SaveChangesAsync();

            var existingRelatedItems = itemEntity.RelatedItems;

            //handle related items
            if (transferItemDto.RelatedItemsSlugs != null)
            {
                //if this item has any related items at all
                if (existingRelatedItems.Count > 0)
                {
                    //remove existing related items, if needed
                    foreach (var existentRelatedItem in existingRelatedItems)
                    {
                        if (!transferItemDto.RelatedItemsSlugs.Contains(existentRelatedItem.RelatedItemSlug))
                        {
                            RemoveRelatedItem(existentRelatedItem.RelatedItemSlug, itemSlug);
                        }
                    }
                    
                    //add a new related item relationship only if it doesn't exist yet
                    foreach (var relatedItemSlug in transferItemDto.RelatedItemsSlugs)
                    {
                        var existingRelatedItem = existingRelatedItems.FirstOrDefault(
                            x => x.ItemSlug == relatedItemSlug || x.RelatedItemSlug == relatedItemSlug
                            );
                        if (existingRelatedItem is null)
                        {
                            await AddRelatedItem(itemEntity.Slug, relatedItemSlug);
                        }
                    }
                    await _dbContext.SaveChangesAsync();
                }
                else
                //if not - add new relations
                {
                    foreach (var relatedItemSlug in transferItemDto.RelatedItemsSlugs)
                    {
                        await AddRelatedItem(itemEntity.Slug, relatedItemSlug);
                    }
                    await _dbContext.SaveChangesAsync();
                }
            } 
            //delete all the existent related items for the current item, if needed
            else if (existingRelatedItems.Count > 0)
            {
                RemoveAllRelatedItems(itemSlug);
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
            RemoveAllRelatedItems(slug);
            
            //removing itemImages
            await _dbContext.ItemImages.Where(x => x.ItemSlug == slug).ExecuteDeleteAsync();
            
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
                Image = new FormFile(itemEntity.SanctuaryRegion.Image, FileName, fileType),
                Description = itemEntity.SanctuaryRegion.Description,
            };
        }
        
        //adding itemType
        var itemTypeDto = new ItemTypeDTO
        {
            Name = itemEntity.ItemType.Name,
            Slug = itemEntity.ItemType.Slug,
            IconSlug = itemEntity.ItemType.IconSlug
        };

        var newItemDto = new ItemDTO
        {
            Title = itemEntity.Title,
            Slug = itemEntity.Slug,
            Position = itemEntity.Position,
            Description = itemEntity.Description,
            RelatedItems = relatedItems,
            ItemType = itemTypeDto,
            ItemTypeSlug = itemEntity.ItemTypeSlug,
            SanctuaryRegion = sanctuaryRegionDto,
            SanctuaryRegionSlug = itemEntity.SanctuaryRegionSlug
        };

        newItemDto.ItemImages = new();

        foreach (var itemImage in itemEntity.ItemImages)
        {
            newItemDto.ItemImages.Add(new FormFile(itemImage.Image, itemImage.Slug, fileType));
        }
  
        return newItemDto;
    }
    
    private async Task<bool> AddRelatedItem(string itemSlug, string relatedItemSlug)
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
        await _dbContext.ItemRelations.AddAsync(itemRelation);
        return true;
    }

    private async Task<bool> RemoveRelatedItem(string itemSlug, string relatedItemSlug)
    {
        var itemRelations = await _dbContext.ItemRelations
            .Where(x => 
                (x.ItemSlug == itemSlug && x.RelatedItemSlug == relatedItemSlug) || 
                (x.RelatedItemSlug == itemSlug && x.ItemSlug == relatedItemSlug))
            .ToListAsync();
        _dbContext.ItemRelations.RemoveRange(itemRelations);
        return true;
    }

    private async Task<bool> RemoveAllRelatedItems(string itemSlug)
    {
        var list = await _dbContext.ItemRelations
            .Where(x => x.ItemSlug == itemSlug || x.RelatedItemSlug == itemSlug)
            .ToListAsync();
        _dbContext.RemoveRange(list);
        
        return true;
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