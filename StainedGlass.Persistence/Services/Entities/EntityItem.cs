using StainedGlass.Persistence.Entities;
using StainedGlass.Persistence.Transfer;
using StainedGlass.Transfer.Mapper;

namespace StainedGlass.Persistence.Services.Entities;

public class EntityItem : Relatable, IPersistenceService
{
    public IEnumerable<Transferable?> GetAllDTOs()
    {
        return EntitiesCollection.Items.Select(e => GetDTO(e.Value));
    }
    
    public Transferable? GetDTOBySlug(string slug)
    {
        if (!EntitiesCollection.Items.ContainsKey(slug))
        {
            return null;
        }
        return GetDTO(EntitiesCollection.Items[slug]);
    }

    public void AddEntity(IPersistanceTransferStruct transferStruct)
    {
        EntitiesCollection.Items.TryAdd(Slug, this);
    }

    public IEnumerable<IPersistanceTransferStruct> GetAllDtos()
    {
        throw new NotImplementedException();
    }

    public IPersistanceTransferStruct? GetDto(string slug)
    {
        throw new NotImplementedException();
    }

    public void RemoveEntity(string slug)
    {
        if (!EntitiesCollection.Items.ContainsKey(slug))
        {
            return;
        }
        
        //remove item from its region
        var sanctuaryRegion = EntitiesCollection.SanctuaryRegions.Values.FirstOrDefault(e => 
            e.Items?.FirstOrDefault(i => i.Slug == Slug) != null
        );
        if (sanctuaryRegion != null)
        {
            sanctuaryRegion.Items?.RemoveWhere(e => e.Slug == Slug);
        }

        //remove this item from its related items as well
        if (RelatedItems.Count > 0)
        {
            foreach (var relatedItem in RelatedItems.Values)
            {
                relatedItem.RelatedItems.Remove(Slug);
            }
        }
        EntitiesCollection.Items.Remove(Slug);
    }

    public void ReplaceEntity(string slug, IPersistanceTransferStruct transferStruct)
    {
        if (!EntitiesCollection.Items.ContainsKey(slug))
        {
            return;
        }
        entity.Slug = slug;
        var oldEntity = EntitiesCollection.Items[slug];
        EntitiesCollection.Items[slug] = (Item)entity;
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
        if (RelatedItems.Count > 0)
        {
            foreach (var relatedItem in RelatedItems.Values)
            {
                //if a related item doesn't have the present item as a related one - add it
                if (!relatedItem.RelatedItems.ContainsKey(Slug))
                {
                    relatedItem.RelatedItems.Add(Slug, (Item)entity);
                }
                else
                {
                    relatedItem.RelatedItems[Slug] = (Item)entity;   
                }
            }
        }
    }

    public Transferable? GetDTO(Entity? entity, bool skipParentElements = false, bool computeRelatedItems = true)
    {
        SanctuaryRegionMapper sanctuaryRegionMapper = new();
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
                sanctuaryRegionMapper.GetDTO(item.SanctuaryRegion, skipChildrenElements : true) as SanctuaryRegionDTO;
            if (sanctuaryRegionDTO != null)
            {
                itemDto.SanctuaryRegionSlug = sanctuaryRegionDTO.Slug;
                itemDto.SanctuaryRegion = sanctuaryRegionDTO;
            }
        }

        if (item.ItemType != null)
        {
            ItemTypeMapper itemTypeMapper = new();
            itemDto.ItemType = itemTypeMapper.GetDTO(item.ItemType) as ItemTypeDTO;
            itemDto.ItemTypeSlug = item.ItemType.Slug;
        }

        if (computeRelatedItems && item.RelatedItems != null)
        {
            foreach (EntityItem relatedItem in item.RelatedItems.Values)
            {
                itemDto.RelatedItemsSlugs.Add(relatedItem.Slug);
                itemDto.RelatedItems.Add(relatedItem.Slug, GetDTO(relatedItem, computeRelatedItems : false) as ItemDTO);
            }
        }

        return itemDto;
    }

    public Entity GetEntity(Transferable transferable)
    {
        ItemDTO itemDto = transferable as ItemDTO;

        SanctuaryRegion sanctuaryRegion = null;
        ItemType itemType = null;

        if (itemDto.SanctuaryRegionSlug != null)
        {
            sanctuaryRegion = EntitiesCollection.SanctuaryRegions[itemDto.SanctuaryRegionSlug];
        }

        if (itemDto.ItemTypeSlug != null)
        {
            itemType = EntitiesCollection.ItemsTypes[itemDto.ItemTypeSlug];
        }

        var window = new EntityItem
        {
            Slug = itemDto.Slug,
            Title = itemDto.Title,
            Description = itemDto.Description,
            Image = itemDto.Image,
            ItemType = itemType,
            SanctuaryRegion = sanctuaryRegion
        };

        if (itemDto.RelatedItemsSlugs != null && itemDto.RelatedItemsSlugs.Count > 0)
        {
            //save current item as a related item to its related items as well
            foreach (string relatedItemsSlug in itemDto.RelatedItemsSlugs)
            {
                if (!EntitiesCollection.Items[relatedItemsSlug].RelatedItems.ContainsKey(window.Slug))
                {
                    EntitiesCollection.Items[relatedItemsSlug].RelatedItems.Add(window.Slug, window);
                }
            }
            
            var relatedItems = EntitiesCollection.Items.Where(
                e => itemDto.RelatedItemsSlugs.Contains(e.Value.Slug)
            ).ToDictionary();
            window.RelatedItems = relatedItems;
        }

        if (sanctuaryRegion != null) 
        {
            sanctuaryRegion.Items.Add(window);
        }

        return window;
    }
}