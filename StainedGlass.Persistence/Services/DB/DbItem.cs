using Microsoft.EntityFrameworkCore;
using StainedGlass.Persistence.Entities;
using StainedGlass.Persistence.Transfer;

namespace StainedGlass.Persistence.Services.DB;

internal class DbItem : DatabasePersistenceService
{
    public override void AddEntity(IPersistanceTransferStruct transferStruct)
    {
        var itemStruct = (ItemDTO)GetDtoFromTransfer(transferStruct);
        var newItem = new Item
        {
            Title = itemStruct.Title,
            Slug = itemStruct.Slug,
            Description = itemStruct.Description,
            Image = itemStruct.Image,
            SanctuaryRegionSlug = itemStruct.SanctuaryRegionSlug,
        };
        
        _dbContext.Items.Add(newItem);
        _dbContext.SaveChanges();

        if (itemStruct.RelatedItemsSlugs != null)
        {
            foreach (var relatedItemsSlug in itemStruct.RelatedItemsSlugs)
            {
                var relation = new ItemRelation
                {
                    ItemSlug = newItem.Slug,
                    RelatedItemSlug = relatedItemsSlug,
                };
                
                _dbContext.ItemRelations.Add(relation);
                _dbContext.SaveChanges();
            }
        }
    }

    public override IEnumerable<IPersistanceTransferStruct> GetAllDtos()
    {
        List<IPersistanceTransferStruct> itemDtos = new List<IPersistanceTransferStruct>();
        
        foreach (var dbItem in _dbContext.Items)
        {
            itemDtos.Add(GetDtoFromEntity(dbItem));
        }
        return itemDtos;
    }

    public override IPersistanceTransferStruct? GetDtoBySlug(string slug)
    {
        var entity = _dbContext.Items.FirstOrDefault(x => x.Slug == slug);
        if (entity is null)
        {
            return null;
        }

        return GetDtoFromEntity(entity);
    }
    
    public override void ReplaceEntity(string slug, IPersistanceTransferStruct transferStruct)
    {
        var item = _dbContext.Items
            .Include(x => x.RelatedItems)
            .FirstOrDefault(x => x.Slug == slug);
        if (item != null)
        {
            var transferItemDto = (ItemDTO)GetDtoFromTransfer(transferStruct);
            item.Title = transferItemDto.Title;
            item.Image = transferItemDto.Image;
            item.Description = transferItemDto.Description;
            item.ItemTypeSlug = transferItemDto.ItemTypeSlug;

            if (transferItemDto.RelatedItemsSlugs != null)
            {
                if (item.RelatedItems.Count > 0)
                {
                    foreach (var relatedItemSlug in transferItemDto.RelatedItemsSlugs)
                    {
                        //if there is no relateditem with such a slug - add one
                        var existingRelatedItem = item.RelatedItems.FirstOrDefault(
                            x => x.Item.Slug == relatedItemSlug
                            );
                        if (existingRelatedItem is null)
                        {
                            var newRelatedItem = _dbContext.Items.FirstOrDefault(x => x.Slug == relatedItemSlug);
                            var itemRelation = new ItemRelation
                            {
                                Item = item,
                                ItemSlug = item.Slug,
                                RelatedItemSlug = relatedItemSlug,
                                RelatedItem = newRelatedItem,
                            };
                            item.RelatedItems.Add(itemRelation);
                        }
                    }
                }
                else
                {
                    foreach (var relatedItemSlug in transferItemDto.RelatedItemsSlugs)
                    {
                        var newRelatedItem = _dbContext.Items.FirstOrDefault(x => x.Slug == relatedItemSlug);
                        var itemRelation = new ItemRelation
                        {
                            Item = item,
                            ItemSlug = item.Slug,
                            RelatedItemSlug = relatedItemSlug,
                            RelatedItem = newRelatedItem,
                        };
                        item.RelatedItems.Add(itemRelation);
                    }
                }
                
            }
        }
    }

    public override void RemoveEntity(string slug)
    {
        var item = _dbContext.Items.FirstOrDefault(x => x.Slug == slug);
        if (item != null)
        {
            _dbContext.Items.Remove(item);
            _dbContext.SaveChanges();
        }
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

        if (itemEntity.RelatedItems != null)
        {
            foreach (var relatedItem in itemEntity.RelatedItems)
            {
                relatedItems.Add(
                    relatedItem.Item.Slug, 
                    new ItemDTO
                    {
                        Title = relatedItem.Item.Title,
                        Slug = relatedItem.Item.Slug,
                    }
                );
            }
        }
        
        //adding itemType
        var itemTypeDto = new ItemTypeDTO
        {
            Name = itemEntity.ItemType.Name,
            Slug = itemEntity.ItemType.Slug,
        };

        return new ItemDTO
        {
            Title = itemEntity.Title,
            Slug = itemEntity.Slug,
            Description = itemEntity.Description,
            Image = itemEntity.Image,
            RelatedItems = relatedItems,
            ItemType = itemTypeDto
        };
    }
}