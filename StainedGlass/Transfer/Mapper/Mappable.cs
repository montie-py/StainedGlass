using StainedGlass.Entities;

namespace StainedGlass.Transfer.Mapper;

public interface Mappable
{
    public Transferable? GetDTOBySlug(string slug);
    public IEnumerable<Transferable?> GetAllDTOs();
    public Entity GetEntity(Transferable transferable);
    public void RemoveEntity(string slug);
}