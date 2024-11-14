using StainedGlass.Entities;
using StainedGlass.Transfer.Mapper;

namespace StainedGlass.Transfer.DTOs;

public class ChurchDTO : Transferable
{
    public required string Slug {get; set;}
    public required string Name {get; set;}
    public required string Image {get; set;}
    public required HashSet<SanctuarySideDTO>? Sides {get; set;}

    public Entity GetEntity(Transferable transferable)
    {
        ChurchDTO churchDTO = transferable as ChurchDTO;

        return new Church
        {
            Slug = churchDTO.Slug,
            Name = churchDTO.Name,
            Image = churchDTO.Image,
            Sides = null
        };
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
        
        return Slug.Equals(other.Slug) && Name.Equals(other.Name) && Image.Equals(other.Image);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Slug, Name, Image);
    }
}