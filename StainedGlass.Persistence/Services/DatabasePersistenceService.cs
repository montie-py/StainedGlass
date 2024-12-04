﻿//the purpose of the class is holding and initializing the _dbContext property
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