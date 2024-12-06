using StainedGlass.Persistence.Entities;
using StainedGlass.Persistence.Transfer;

namespace StainedGlass.Persistence.Services.Entities;

public interface IInMemoryService
{
    public IEntity GetEntity(IPersistanceTransferStruct transferable);
}