using StainedGlass.Entities;
using StainedGlass.Entities.Transfer;
using StainedGlass.Transfer.DTOs;

namespace StainedGlass.Transfer.Mapper;

internal class ItemMapper : Relatable
{
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

    public Transferable? GetDTOBySlug(string slug)
    {
        if (!EntitiesCollection.Items.ContainsKey(slug))
        {
            return null;
        }
        return GetDTO(EntitiesCollection.Items[slug]);
    }

    public IEnumerable<Transferable?> GetAllDTOs()
    {
        return EntitiesCollection.Items.Select(e => GetDTO(e.Value));
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
                EntitiesCollection.Items[relatedItemsSlug].RelatedItems.Add(window.Slug, window);
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
        EntitiesCollection.Items[slug].Remove();
    }
}