using Microsoft.AspNetCore.Http;
using StainedGlass.Transfer.Mapper;

namespace StainedGlass.Transfer.DTOs;

public class ItemImageDTO : Transferable
{
    public string Slug { get; set; }
    public string ItemSlug { get; set; }
    public IFormFile Image { get; set; }

    public static implicit operator Persistence.Transfer.ItemImageDTO(ItemImageDTO itemImageDTO)
    {
        return new Persistence.Transfer.ItemImageDTO
        {
            Slug = itemImageDTO.Slug,
            ItemSlug = itemImageDTO.ItemSlug,
            Image = itemImageDTO.Image,
        };
    }

    public Mappable GetMapper()
    {
        return new ItemImageMapper();
    }
}