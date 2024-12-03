using StainedGlass.Persistence.Entities;
using StainedGlass.Persistence.Transfer;

namespace StainedGlass.Persistence.Services;

internal abstract class DatabasePersistenceService : IPersistenceService
{
    protected readonly AppDbContext _dbContext;

    public DatabasePersistenceService()
    {
        _dbContext = new AppDbContext();
    }
    public abstract void AddEntity(IPersistanceTransferStruct transferStruct);
    public abstract IEnumerable<IPersistanceTransferStruct> GetAllDtos();
    public abstract IPersistanceTransferStruct? GetDto(string slug);
    public abstract void RemoveEntity(string slug);
    public abstract void ReplaceEntity(string slug, IPersistanceTransferStruct transferStruct);
}