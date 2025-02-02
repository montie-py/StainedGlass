using StainedGlass.Transfer.Mapper;

namespace StainedGlass.Transfer.DTOs;

public class ItemTypeDTO : Transferable
{
    public string Slug { get; set; }
    public string Name { get; set; }
    public string IconSlug { get; set; }
    
    public ICollection<ItemDTO> Items { get; set; }

    public static implicit operator Persistence.Transfer.ItemTypeDTO(ItemTypeDTO itemTypeDto)
    {
        return new Persistence.Transfer.ItemTypeDTO
        {
            Name = itemTypeDto.Name,
            Slug = itemTypeDto.Slug,
            IconSlug = itemTypeDto.IconSlug,
        };
    }

    public Mappable GetMapper()
    {
        return new ItemTypeMapper();
    }

    public override bool Equals(object? obj)
    {
        if (obj == null || GetType() != obj.GetType())
            return false;
        var other = (ItemTypeDTO)obj;
        
        return Slug == other.Slug && Name == other.Name;
    }
}