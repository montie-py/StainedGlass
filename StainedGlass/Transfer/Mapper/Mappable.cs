using StainedGlass.Persistence.Services;
using StainedGlass.Persistence.Templates;

namespace StainedGlass.Transfer.Mapper;

public interface Mappable
{
    public abstract void SetInstance(IPersistenceTemplate template);
    public Task<Transferable?> GetDTOBySlug(string slug);
    public Task<ICollection<Transferable?>> GetAllDTOs();
    public Task<bool> RemoveEntity(string slug);
    public Task<bool> SaveEntity(Transferable transferable);
    public Task<bool> ReplaceEntity(string slug, Transferable transferable);
}