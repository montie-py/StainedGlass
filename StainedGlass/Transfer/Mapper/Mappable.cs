using StainedGlass.Persistence.Services;
using StainedGlass.Persistence.Templates;

namespace StainedGlass.Transfer.Mapper;

public interface Mappable
{
    public abstract void SetInstance(IPersistenceTemplate template);
    public Transferable? GetDTOBySlug(string slug);
    public IEnumerable<Transferable?> GetAllDTOs();
    public void RemoveEntity(string slug);
    public void SaveEntity(Transferable transferable);
    public void ReplaceEntity(string slug, Transferable transferable);
}