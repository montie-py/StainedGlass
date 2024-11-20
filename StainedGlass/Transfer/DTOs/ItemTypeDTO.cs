using StainedGlass.Entities;
using StainedGlass.Transfer.Mapper;

namespace StainedGlass.Transfer.DTOs;

public class ItemTypeDTO : Transferable
{
    public string Slug { get; set; }
    public string Name { get; set; }
    
    public Entity GetEntity(Transferable transferable)
    {
        return (new ItemTypeMapper()).GetEntity(transferable);
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