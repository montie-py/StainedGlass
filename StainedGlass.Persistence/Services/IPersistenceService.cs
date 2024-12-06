using StainedGlass.Persistence.Entities;
using StainedGlass.Persistence.Transfer;

namespace StainedGlass.Persistence.Services;

public interface IPersistenceService
{
    public void AddEntity(IPersistanceTransferStruct transferStruct);
    public IEnumerable<IPersistanceTransferStruct> GetAllDtos();
    public IPersistanceTransferStruct? GetDtoBySlug(string slug);
    public void RemoveEntity(string slug);
    public void ReplaceEntity(string slug, IPersistanceTransferStruct transferStruct);
}