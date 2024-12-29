using StainedGlass.Persistence.Entities;
using StainedGlass.Persistence.Transfer;
using StainedGlass.Transfer.Mapper;

namespace StainedGlass.Persistence.Services.Entities;

public class EntityItem : IRelatable, IPersistenceService
{

    public async Task<bool> AddEntity(IPersistanceTransferStruct transferStruct)
    {
        var itemDto = (ItemDTO)transferStruct;
        var entity = GetEntity(transferStruct) as Item;
        EntitiesCollection.Items.TryAdd(itemDto.Slug, entity);
        return true;
    }

    public async Task<ICollection<IPersistanceTransferStruct>> GetAllDtos()
    {
        return EntitiesCollection.Items.Select(e => GetDTOForEntity(e.Value)).ToList();
    }

    public async Task<IPersistanceTransferStruct?> GetDtoBySlug(string slug)
    {
        if (!EntitiesCollection.Items.ContainsKey(slug))
        {
            return null;
        }
        return GetDTOForEntity(EntitiesCollection.Items[slug]);
    }

    public async Task<bool> RemoveEntity(string slug)
    {
        if (!EntitiesCollection.Items.ContainsKey(slug))
        {
            return false;
        }
        
        var item = EntitiesCollection.Items[slug];
        
        //remove item from its region
        var sanctuaryRegion = EntitiesCollection.SanctuaryRegions.Values.FirstOrDefault(e => 
            e.Items?.FirstOrDefault(i => i.Slug == slug) != null
        );
        if (sanctuaryRegion is null)
        {
            return false;
        }
        
        ((HashSet<Item>)sanctuaryRegion.Items)?.RemoveWhere(e => e.Slug == slug);

        //remove this item from its related items as well
        if (item.RelatedItems.Count > 0)
        {
            foreach (var relatedItem in ((Dictionary<string, Item>)item.RelatedItems).Values)
            {
                var itemRelations = relatedItem.RelatedItems as List<ItemRelation>;
                itemRelations.RemoveAll(e => e.RelatedItemSlug == slug);
            }
        }
        EntitiesCollection.Items.Remove(slug);
        return true;
    }

    public async Task<bool> ReplaceEntity(string slug, IPersistanceTransferStruct transferStruct)
    {
        if (!EntitiesCollection.Items.ContainsKey(slug))
        {
            return false;
        }

        var entity = GetEntity(transferStruct) as Item;
        entity.Slug = slug;
        var oldEntity = EntitiesCollection.Items[slug];
        EntitiesCollection.Items[slug] = entity;
        //if old entity has an assigned itemType - new one cannot lack one
        if (EntitiesCollection.Items[slug].ItemType is null)
        {
            EntitiesCollection.Items[slug].ItemType = oldEntity.ItemType;
        }
        //if old entity has an assigned region - new one cannot lack one
        if (EntitiesCollection.Items[slug].SanctuaryRegion is null)
        {
            EntitiesCollection.Items[slug].SanctuaryRegion = oldEntity.SanctuaryRegion;
        }

        //if the item has related items - update this related item with the newer version
        if (entity.RelatedItems.Count > 0)
        {
            foreach (var relatedItem in ((Dictionary<string, Item>)entity.RelatedItems).Values)
            {
                //if a related item doesn't have the present item as a related one - add it
                
                if (relatedItem.RelatedItems.FirstOrDefault(e => e.RelatedItemSlug == slug) == null)
                {
                    var itemRelation = new ItemRelation
                    {
                        Item = relatedItem,
                        ItemSlug = relatedItem.Slug,
                        RelatedItemSlug = entity.Slug,
                        RelatedItem = entity
                    };
                    relatedItem.RelatedItems.Add(itemRelation);   
                }
                else
                {
                    relatedItem.RelatedItems
                        .FirstOrDefault(e => e.RelatedItemSlug == slug)
                        .RelatedItem = entity;
                }
            }
        }

        return true;
    }

    public IPersistanceTransferStruct? GetDTOForEntity(
        IEntity? entity, 
        bool skipParentElements = false, 
        bool computeRelatedItems = true
        )
    {
        EntitySanctuaryRegion entitySanctuaryRegion = new();
        Item item = entity as Item;

        var itemDto = new ItemDTO
        {
            Slug = item.Slug,
            Title = item.Title,
            Description = item.Description,
            Image = item.Image,
        };

        if (!skipParentElements)
        {
            SanctuaryRegionDTO? sanctuaryRegionDTO = 
                entitySanctuaryRegion.GetDTOForEntity(
                    item.SanctuaryRegion, 
                    skipChildrenElements : true
                    ) as SanctuaryRegionDTO?;
            if (sanctuaryRegionDTO != null)
            {
                itemDto.SanctuaryRegionSlug = sanctuaryRegionDTO.Value.Slug;
                itemDto.SanctuaryRegion = sanctuaryRegionDTO;
            }
        }

        if (item.ItemType != null)
        {
            EntityItemType entityItemType = new();
            itemDto.ItemType = (ItemTypeDTO)entityItemType.GetDTOForEntity(item.ItemType);
            itemDto.ItemTypeSlug = item.ItemType.Slug;
        }

        if (computeRelatedItems && item.RelatedItems != null)
        {
            foreach (ItemRelation relatedItem in item.RelatedItems)
            {
                itemDto.RelatedItemsSlugs.Add(relatedItem.ItemSlug);
                itemDto.RelatedItems.Add(
                    relatedItem.ItemSlug, 
                    (ItemDTO)GetDTOForEntity(relatedItem.RelatedItem, computeRelatedItems : false)
                    );
            }
        }

        return itemDto;
    }

    public IEntity GetEntity(IPersistanceTransferStruct transferable)
    {
        ItemDTO? itemDto = transferable as ItemDTO?;

        if (itemDto == null)
        {
            return null;
        }

        SanctuaryRegion sanctuaryRegion = null;
        ItemType itemType = null;

        if (itemDto.Value.SanctuaryRegionSlug != null)
        {
            sanctuaryRegion = EntitiesCollection.SanctuaryRegions[itemDto.Value.SanctuaryRegionSlug];
        }

        if (itemDto.Value.ItemTypeSlug != null)
        {
            itemType = EntitiesCollection.ItemsTypes[itemDto.Value.ItemTypeSlug];
        }

        var entity = new Item
        {
            Slug = itemDto.Value.Slug,
            Title = itemDto.Value.Title,
            Description = itemDto.Value.Description,
            Image = itemDto.Value.Image,
            ItemType = itemType,
            SanctuaryRegion = sanctuaryRegion
        };

        if (itemDto.Value.RelatedItemsSlugs != null && itemDto.Value.RelatedItemsSlugs.Count > 0)
        {
            //save current item as a related item to its related items as well
            foreach (string relatedItemsSlug in itemDto.Value.RelatedItemsSlugs)
            {
                var currentRelatedItem =
                    EntitiesCollection.Items[relatedItemsSlug].RelatedItems.FirstOrDefault(
                        e => e.RelatedItemSlug == itemDto.Value.Slug);
                if (currentRelatedItem is null)
                {
                    EntitiesCollection.Items[relatedItemsSlug].RelatedItems.Add(
                        new ItemRelation
                        {
                            Item = EntitiesCollection.Items[relatedItemsSlug],
                            ItemSlug = EntitiesCollection.Items[relatedItemsSlug].Slug,
                            RelatedItem = entity,
                            RelatedItemSlug = entity.Slug
                        }
                            );
                }
            }
            
            
            var relatedItems = EntitiesCollection.Items.Where(
                e => itemDto.Value.RelatedItemsSlugs.Contains(e.Value.Slug)
            );
            entity.RelatedItems = (ICollection<ItemRelation>)relatedItems;
        }

        if (sanctuaryRegion != null) 
        {
            sanctuaryRegion.Items.Add(entity);
        }

        return entity;
    }
}