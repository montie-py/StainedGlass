using StainedGlass.Transfer.Mapper;

namespace StainedGlass.Transfer.DTOs;

public class ItemDTO : Transferable
{
    public string Slug {get; set;}
    public string Title{get; set;}
    public string Description{get; set;}
    public string Image{get; set;}
    public string ItemTypeSlug {get; set;}
    public ItemTypeDTO ItemType{get; set;}
    public List<ItemImageDTO> ItemImages{get; set;}

    public string? SanctuaryRegionSlug { get; set; }
    public SanctuaryRegionDTO? SanctuaryRegion { set; get; } = new();
    public Dictionary<string, ItemDTO>? RelatedItems { get; set; } = new();
    public HashSet<string>? RelatedItemsSlugs { get; set; } = new();

    public static implicit operator Persistence.Transfer.ItemDTO(ItemDTO itemDto)
    {
        var returnItemDto = new Persistence.Transfer.ItemDTO
        {
            Title = itemDto.Title,
            Slug = itemDto.Slug,
            Description = itemDto.Description,
            ItemTypeSlug = itemDto.ItemTypeSlug,
            SanctuaryRegionSlug = itemDto.SanctuaryRegionSlug,
            RelatedItemsSlugs = itemDto.RelatedItemsSlugs,
        };

        foreach (var itemImage in itemDto.ItemImages)
        {
            returnItemDto.ItemImages.Add(itemImage);
        }
        
        return returnItemDto;
    }

    public Mappable GetMapper()
    {
        return new ItemMapper();
    }

    public override bool Equals(object? obj)
    {
        if (obj == null || GetType() != obj.GetType()) 
        { return false; }
        
        var other = (ItemDTO)obj;

        return Slug == other.Slug
               && Title == other.Title
               && Description == other.Description
               && Image == other.Image;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Slug, Title, Description, Image);
    }
}