using StainedGlass.Persistence.Services;
using StainedGlass.Persistence.Templates;
using StainedGlass.Persistence.Transfer;

namespace StainedGlass.Transfer.Mapper;

internal abstract class Mapper : Mappable
{
    protected IPersistenceService _persistenceService;
    public abstract void SetInstance(IPersistenceTemplate template);
    public abstract Task<Transferable?> GetDTOBySlug(string slug);

    public abstract Task<ICollection<Transferable?>> GetAllDTOs();

    public abstract Task<bool> RemoveEntity(string slug);
    public abstract Task<bool> SaveEntity(Transferable transferable);
    public abstract Task<bool> ReplaceEntity(string slug, Transferable transferable);
    protected abstract Transferable GetDtoFromTransferable(IPersistanceTransferStruct transferStruct);
}