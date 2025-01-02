using Microsoft.AspNetCore.Http;
using StainedGlass.Transfer.Mapper;

namespace StainedGlass.Transfer.DTOs;

public class SanctuaryRegionDTO : Transferable
{
    public string Slug {get; set;}
    public string Name {get; set;}
    public string Description {get; set;}
    public IFormFile Image {get; set;}
    public HashSet<ItemDTO>? Items { get; set; } = new();
    public string SanctuarySideSlug {get; set;}
    public SanctuarySideDTO SanctuarySide {get; set;}

    public static implicit operator Persistence.Transfer.SanctuaryRegionDTO(SanctuaryRegionDTO sanctuaryRegion)
    {
        return new Persistence.Transfer.SanctuaryRegionDTO
        {
            Name = sanctuaryRegion.Name,
            Description = sanctuaryRegion.Description,
            Image = sanctuaryRegion.Image,
            Slug = sanctuaryRegion.Slug,
            SanctuarySideSlug = sanctuaryRegion.SanctuarySideSlug,
        };
    }

    public Mappable GetMapper()
    {
        return new SanctuaryRegionMapper();
    }

    public override bool Equals(object? obj)
    {
        if (obj == null || GetType() != obj.GetType()) 
        { return false; }
        
        var other = (SanctuaryRegionDTO)obj;
        
        return Name == other.Name && Image == other.Image && Slug == other.Slug && Description == other.Description;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Name, Image, Slug);
    }
}