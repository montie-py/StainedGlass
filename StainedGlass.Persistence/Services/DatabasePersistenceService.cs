//the purpose of the class is holding and initializing the _dbContext property

using Microsoft.Extensions.Logging;
using StainedGlass.Persistence.Entities;
using StainedGlass.Persistence.Transfer;

namespace StainedGlass.Persistence.Services;

internal abstract class DatabasePersistenceService : IPersistenceService
{
    protected readonly AppDbContext _dbContext;
    protected readonly string fileType = "image/jpeg";
    protected string FileName { get; init; }

    public DatabasePersistenceService()
    {
        _dbContext = new AppDbContext();
    }
    public abstract Task<bool> AddEntity(IPersistanceTransferStruct transferStruct);
    public abstract Task<ICollection<IPersistanceTransferStruct>> GetAllDtos();
    public abstract Task<IPersistanceTransferStruct?> GetDtoBySlug(string slug, bool includeChildrenToTheResponse);
    public abstract Task<bool> RemoveEntity(string slug);
    public abstract Task<bool> ReplaceEntity(string slug, IPersistanceTransferStruct transferStruct);
    protected abstract IPersistanceTransferStruct GetDtoFromTransfer(IPersistanceTransferStruct transferStruct);
    protected abstract IPersistanceTransferStruct GetDtoFromEntity(IEntity entity);
}