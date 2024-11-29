using StainedGlass.Persistence.Entities;
using StainedGlass.Persistence.Transfer;

namespace StainedGlass.Persistence.Services.DB;

internal class DbItemType : DatabasePersistenceService
{
    public override void AddEntity(IPersistanceTransferStruct transferStruct)
    {
        var itemStruct = (ItemTypeDTO)transferStruct;
        var itemTypeEntity = new ItemType
        {
            Name = itemStruct.Name,
            Slug = itemStruct.Slug,
        };
        _dbContext.ItemTypes.Add(itemTypeEntity);
        _dbContext.SaveChanges();
    }

    public override List<IEntity> GetEntities()
    {
        return new List<IEntity>(_dbContext.ItemTypes);
    }
}