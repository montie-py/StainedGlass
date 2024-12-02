﻿using Microsoft.EntityFrameworkCore;
using StainedGlass.Persistence.Entities;
using StainedGlass.Persistence.Transfer;

namespace StainedGlass.Persistence.Services.DB;

internal class DbItem : DatabasePersistenceService
{
    public override void AddEntity(IPersistanceTransferStruct transferStruct)
    {
        var itemStruct = (ItemDTO)transferStruct;
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
            var relatedItems = dbItem.RelatedItems;
            
            if (relatedItems == null)
            {
                //if relateditems == null - use local GetRelatedItemsBySlug() method
            }
            
            var relatedItemsDtos = new Dictionary<string, ItemDTO>();

            //adding related items
            if (relatedItems != null)
            {
                foreach (var relatedItem in relatedItems)
                {
                    var relatedItemDto = new ItemDTO
                    {
                        Title = relatedItem.Item.Title,
                        Slug = relatedItem.Item.Slug,
                    };
                    relatedItemsDtos.Add(relatedItemDto.Slug, relatedItemDto);
                }   
            }
            
            //adding itemtype
            var itemTypeDto = new ItemTypeDTO
            {
                Name = dbItem.ItemType.Name,
                Slug = dbItem.ItemType.Slug,
            };
            var itemDto = new ItemDTO
            {
                Title = dbItem.Title,
                Slug = dbItem.Slug,
                Description = dbItem.Description,
                Image = dbItem.Image,
                ItemType = itemTypeDto,
                RelatedItems = relatedItemsDtos
            };
            itemDtos.Add(itemDto);
        }
        return itemDtos;
    }

    public override IPersistanceTransferStruct? GetDto(string slug)
    {
        var entity = _dbContext.Items.FirstOrDefault(x => x.Slug == slug);
        if (entity is null)
        {
            return null;
        }

        //adding related items
        var relatedItems = new Dictionary<string, ItemDTO>();

        if (entity.RelatedItems != null)
        {
            foreach (var relatedItem in entity.RelatedItems)
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
            Name = entity.ItemType.Name,
            Slug = entity.ItemType.Slug,
        };

        return new ItemDTO
        {
            Title = entity.Title,
            Slug = entity.Slug,
            Description = entity.Description,
            Image = entity.Image,
            RelatedItems = relatedItems,
            ItemType = itemTypeDto
        };
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
}