using StainedGlass.Entities;

namespace StainedGlass.Transfer.Mapper;

public interface Mappable
{
    public Transferable? GetDTOBySlug(string slug);
    public Entity GetEntity(Transferable transferable);
}