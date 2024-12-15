﻿using Microsoft.EntityFrameworkCore;
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
            ItemTypeSlug = itemStruct.ItemTypeSlug
        };
        
        _dbContext.Items.Add(newItem);
        _dbContext.SaveChanges();

        if (itemStruct.RelatedItemsSlugs != null)
        {
            foreach (var relatedItemsSlug in itemStruct.RelatedItemsSlugs)
            {
                AddRelatedItem(newItem.Slug, relatedItemsSlug);
            }
            _dbContext.SaveChanges();
        }
    }

    public override ICollection<IPersistanceTransferStruct> GetAllDtos()
    {
        List<IPersistanceTransferStruct> itemDtos = new List<IPersistanceTransferStruct>();
        
        foreach (var dbItem in _dbContext.Items
                     .Include(i => i.ItemType)
                     .Include(i => i.SanctuaryRegion)
                     .Include(i => i.RelatedItems)
                     .ThenInclude(ir => ir.RelatedItem))
        {
            itemDtos.Add(GetDtoFromEntity(dbItem));
        }
        return itemDtos;
    }

    public override IPersistanceTransferStruct? GetDtoBySlug(string slug)
    {
        var entity = _dbContext.Items
            .Include(i => i.ItemType)
            .Include(i => i.SanctuaryRegion)
            .Include(i => i.RelatedItems)
            .ThenInclude(ir => ir.RelatedItem)
            .FirstOrDefault(x => x.Slug == slug);
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
            item.SanctuaryRegionSlug = item.SanctuaryRegionSlug;
            
            _dbContext.SaveChanges();

            if (transferItemDto.RelatedItemsSlugs != null)
            {
                //if this item has any related items at all
                if (item.RelatedItems.Count > 0)
                {
                    foreach (var relatedItemSlug in transferItemDto.RelatedItemsSlugs)
                    {
                        //if there is no relateditem with such a slug - add one
                        var existingRelatedItem = item.RelatedItems.FirstOrDefault(
                            x => x.ItemSlug == relatedItemSlug || x.RelatedItemSlug == relatedItemSlug
                            );
                        if (existingRelatedItem is null)
                        {
                            AddRelatedItem(item.Slug, relatedItemSlug);
                        }
                    }
                    _dbContext.SaveChanges();
                }
                else
                //if not - add new relations
                {
                    foreach (var relatedItemSlug in transferItemDto.RelatedItemsSlugs)
                    {
                        AddRelatedItem(item.Slug, relatedItemSlug);
                    }
                    _dbContext.SaveChanges();
                }
                
            }
        }
    }

    public override void RemoveEntity(string slug)
    {
        var item = _dbContext.Items.FirstOrDefault(x => x.Slug == slug);
        if (item != null)
        {
            //removing itemRelation first
            var list = _dbContext.ItemRelations
                .Where(x => x.ItemSlug == slug || x.RelatedItemSlug == slug)
                .ToList();
            _dbContext.RemoveRange(list);
            
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
                        Image = relatedItem.RelatedItem.Image,
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
                Image = itemEntity.SanctuaryRegion.Image,
                Description = itemEntity.SanctuaryRegion.Description,
            };
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
            ItemType = itemTypeDto,
            ItemTypeSlug = itemEntity.ItemTypeSlug,
            SanctuaryRegion = sanctuaryRegionDto,
            SanctuaryRegionSlug = itemEntity.SanctuaryRegionSlug
        };
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
}