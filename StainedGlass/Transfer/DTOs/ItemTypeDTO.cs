using StainedGlass.Entities;
using StainedGlass.Transfer.Mapper;

namespace StainedGlass.Transfer.DTOs;

public class ItemTypeDTO : Transferable
{
    public string Slug { get; set; }
    public string Name { get; set; }
    
    public Entity GetEntity(Transferable transferable)
    {
        return (new ItemMapper()).GetEntity(transferable);
    }

    public Mappable GetMapper()
    {
        return new ItemTypeMapper();
    }
}