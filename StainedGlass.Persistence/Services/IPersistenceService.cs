using StainedGlass.Persistence.Entities;
using StainedGlass.Persistence.Transfer;

namespace StainedGlass.Persistence.Services;

internal interface IPersistenceService
{
    internal void AddEntity(IPersistanceTransferStruct transferStruct);
    internal List<IEntity> GetEntities();
}