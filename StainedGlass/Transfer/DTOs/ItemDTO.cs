using StainedGlass.Entities;
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

    public string? SanctuaryRegionSlug { get; set; }
    public SanctuaryRegionDTO? SanctuaryRegion { set; get; } = new();
    public Dictionary<string, ItemDTO>? RelatedItems { get; set; } = new();
    public HashSet<string>? RelatedItemsSlugs { get; set; } = new();

    public static implicit operator Persistence.Transfer.ItemDTO(ItemDTO itemDto)
    {
        Persistence.Transfer.ItemTypeDTO itemTypeDto = new();
        Persistence.Transfer.SanctuaryRegionDTO sanctuaryRegionDto = new();
        if (itemDto.ItemType != null)
        {
            itemTypeDto = new Persistence.Transfer.ItemTypeDTO
            {
                Name = itemDto.ItemType.Name,
                Slug = itemDto.Slug
            };
        }
        
        return new Persistence.Transfer.ItemDTO
        {
            Title = itemDto.Title,
            Description = itemDto.Description,
            Image = itemDto.Image,
            ItemType = itemTypeDto,
            SanctuaryRegionSlug = itemDto.SanctuaryRegionSlug,
            RelatedItemsSlugs = itemDto.RelatedItemsSlugs,
        };
    }


    public Entity GetEntity(Transferable transferable)
    {
        return (new ItemMapper()).GetEntity(transferable);
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