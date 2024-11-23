using StainedGlass.Entities;
using StainedGlass.Transfer.Mapper;

namespace StainedGlass.Transfer.DTOs;

public class ChurchDTO : Transferable
{
    public string Slug {get; set;}
    public string Name {get; set;}
    public string Description {get; set;}
    public string Image {get; set;}
    public  HashSet<SanctuarySideDTO>? Sides { get; set; } = new();

    public Entity GetEntity(Transferable transferable)
    {
        return (new ChurchMapper()).GetEntity(transferable);
    }
    
    public Mappable GetMapper()
    {
        return new ChurchMapper();
    }

    public override bool Equals(object? obj)
    {
        if (obj == null || GetType() != obj.GetType()) 
        { return false; }

        var other = (ChurchDTO)obj;
        
        return Slug.Equals(other.Slug) 
            && Name.Equals(other.Name) 
            && Image.Equals(other.Image) 
            && Description.Equals(other.Description);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Slug, Name, Image);
    }
}