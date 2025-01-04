using Microsoft.AspNetCore.Http;

namespace StainedGlass.Transfer.DTOs;

public class ItemImageDTO
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
}