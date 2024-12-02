using StainedGlass.Persistence.Entities;
using StainedGlass.Persistence.Transfer;

namespace StainedGlass.Persistence.Services;

public interface IPersistenceService
{
    public void AddEntity(IPersistanceTransferStruct transferStruct);
    public IEnumerable<IPersistanceTransferStruct> GetAllDtos();
    public IPersistanceTransferStruct? GetDto(string slug);
}