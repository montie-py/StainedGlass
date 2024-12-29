using StainedGlass.Persistence.Entities;
using StainedGlass.Persistence.Transfer;

namespace StainedGlass.Persistence.Services;

public interface IPersistenceService
{
    public Task<bool> AddEntity(IPersistanceTransferStruct transferStruct);
    public Task<ICollection<IPersistanceTransferStruct>> GetAllDtos();
    public Task<IPersistanceTransferStruct?> GetDtoBySlug(string slug);
    public Task<bool> RemoveEntity(string slug);
    public Task<bool> ReplaceEntity(string slug, IPersistanceTransferStruct transferStruct);
}