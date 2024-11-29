using StainedGlass.Persistence.Entities;
using StainedGlass.Persistence.Transfer;

namespace StainedGlass.Persistence.Services;

public interface IPersistenceService
{
    internal void AddEntity(IPersistanceTransferStruct transferStruct);
    internal IEnumerable<IPersistanceTransferStruct> GetAllDtos();
}