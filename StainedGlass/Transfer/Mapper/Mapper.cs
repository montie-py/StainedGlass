using StainedGlass.Persistence.Services;
using StainedGlass.Persistence.Templates;
using StainedGlass.Persistence.Transfer;

namespace StainedGlass.Transfer.Mapper;

internal abstract class Mapper : Mappable
{
    protected IPersistenceService _persistenceService;
    public abstract void SetInstance(IPersistenceTemplate template);
    public abstract Transferable? GetDTOBySlug(string slug);

    public abstract ICollection<Transferable?> GetAllDTOs();

    public abstract void RemoveEntity(string slug);
    public abstract void SaveEntity(Transferable transferable);
    public abstract void ReplaceEntity(string slug, Transferable transferable);
    protected abstract Transferable GetDtoFromTransferable(IPersistanceTransferStruct transferStruct);
}