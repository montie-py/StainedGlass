
using StainedGlass.Persistence.Templates;
using StainedGlass.Transfer.DTOs;

namespace StainedGlass.Transfer.Mapper;

internal class ItemMapper : Mapper, Relatable
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
            foreach (Item relatedItem in item.RelatedItems.Values)
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

        var window = new Item
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

    public void RemoveEntity(string slug)
    {
        if (EntitiesCollection.Items.ContainsKey(slug))
        {
            EntitiesCollection.Items[slug].Remove();
        }
    }
}