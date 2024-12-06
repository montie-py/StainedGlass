using StainedGlass.Transfer.Mapper;

namespace StainedGlass.Transfer.DTOs;

public class SanctuarySideDTO : Transferable
{
    public string Slug { get; set; }
    public string Name { get; set; }
    public List<SanctuaryRegionDTO> Regions { get; set; } = new();
    public string ChurchSlug { get; set; }
    public ChurchDTO? Church { get; set; }

    public static implicit operator Persistence.Transfer.SanctuarySideDTO(SanctuarySideDTO sanctuarySideDto)
    {
        return new Persistence.Transfer.SanctuarySideDTO()
        {
            Name = sanctuarySideDto.Name,
            Slug = sanctuarySideDto.Slug,
            ChurchSlug = sanctuarySideDto.ChurchSlug
        };
    }

    public Mappable GetMapper()
    {
        return new SanctuarySideMapper();
    }

    public override bool Equals(object? obj)
    {
        if (obj == null || GetType() != obj.GetType()) 
        { return false; }
        
        var other = (SanctuarySideDTO)obj;
        
        return Slug == other.Slug && Name == other.Name && Regions.SequenceEqual(other.Regions);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Slug, Name, Regions);
    }
}