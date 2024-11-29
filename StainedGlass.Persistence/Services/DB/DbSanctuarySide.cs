using StainedGlass.Persistence.Entities;
using StainedGlass.Persistence.Transfer;

namespace StainedGlass.Persistence.Services.DB;

internal class DbSanctuarySide : DatabasePersistenceService
{
    public override void AddEntity(IPersistanceTransferStruct transferStruct)
    {
        var itemStruct = (SanctuarySideDTO)transferStruct;
        //todo: create an entity from this struct
    }

    public override List<IEntity> GetEntities()
    {
        throw new NotImplementedException();
    }
}