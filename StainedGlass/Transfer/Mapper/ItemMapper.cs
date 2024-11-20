using StainedGlass.Entities;
using StainedGlass.Entities.Transfer;
using StainedGlass.Transfer.DTOs;

namespace StainedGlass.Transfer.Mapper;

internal class ItemMapper : Relatable
{
    public Transferable? GetDTO(Entity? entity, bool computeRelatedItems = true)
    {
        SanctuaryRegionMapper sanctuaryRegionMapper = new();
        Item item = entity as Item;
        SanctuaryRegionDTO? sanctuaryRegionDTO = 
            sanctuaryRegionMapper.GetDTO(item.SanctuaryRegion) as SanctuaryRegionDTO;

        var itemDto = new ItemDTO
        {
            Slug = item.Slug,
            Title = item.Title,
            Description = item.Description,
            Image = item.Image,
            SanctuaryRegion = sanctuaryRegionDTO
        };

        if (sanctuaryRegionDTO != null)
        {
            itemDto.SanctuaryRegionSlug = sanctuaryRegionDTO.Slug;
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
        return GetDTO(EntitiesCollection.Items[slug]);
    }

    public IEnumerable<Transferable?> GetAllDTOs()
    {
        return EntitiesCollection.Items.Select(e => GetDTO(e.Value));
    }

    public Entity GetEntity(Transferable transferable)
    {
        ItemDTO itemDto = transferable as ItemDTO;

        SanctuaryRegion sanctuaryRegion = EntitiesCollection.SanctuaryRegions[itemDto.SanctuaryRegionSlug];

        var window = new Item
        {
            Slug = itemDto.Slug,
            Title = itemDto.Title,
            Description = itemDto.Description,
            Image = itemDto.Image,
            SanctuaryRegion = sanctuaryRegion
        };

        if (itemDto.RelatedItemsSlugs != null)
        {
            var relatedItems = EntitiesCollection.Items.Where(
                e => itemDto.RelatedItemsSlugs.Contains(e.Value.Slug)
                ).ToDictionary();
            window.RelatedItems = relatedItems;
            
            //save current item as a related item to its related items as well
            foreach (Item relatedItem in relatedItems.Values)
            {
                relatedItem.RelatedItems.Add(window.Slug, window);
            }
        }

        if (sanctuaryRegion != null) 
        {
            sanctuaryRegion.Windows.Add(window);
        }

        return window;
    }

    public void RemoveEntity(string slug)
    {
        EntitiesCollection.Items.Remove(slug);
    }
}