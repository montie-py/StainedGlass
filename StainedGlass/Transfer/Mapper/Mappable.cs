using StainedGlass.Persistence.Services;
using StainedGlass.Persistence.Templates;

namespace StainedGlass.Transfer.Mapper;

public interface Mappable
{
    public abstract void SetInstance(IPersistenceTemplate template);
    public Transferable? GetDTOBySlug(string slug);
    public IEnumerable<Transferable?> GetAllDTOs();
    public Entity GetEntity(Transferable transferable);
    public void RemoveEntity(string slug);
    public void SaveEntity(Transferable transferable);
}