using StainedGlass.Entities;
using StainedGlass.Transfer.Mapper;

namespace StainedGlass.Transfer.DTOs;

public class SanctuaryRegionDTO : Transferable
{
    public string Slug {get; set;}
    public string Name {get; set;}
    public string Description {get; set;}
    public string Image {get; set;}
    public HashSet<ItemDTO>? Windows {get; set;}
    public string SanctuarySideSlug {get; set;}
    public SanctuarySideDTO SanctuarySide {get; set;}

    public Entity GetEntity(Transferable transferable)
     {
        return (new SanctuaryRegionMapper()).GetEntity(transferable); 
     }

    public Mappable GetMapper()
    {
        return new SanctuaryRegionMapper();
    }

    public override bool Equals(object? obj)
    {
        if (obj == null || GetType() != obj.GetType()) 
        { return false; }
        
        var other = (SanctuaryRegion)obj;
        
        return Name == other.Name && Image == other.Image && Slug == other.Slug;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Name, Image, Slug);
    }
}