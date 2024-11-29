using StainedGlass.Persistence.Entities;
using StainedGlass.Persistence.Transfer;

namespace StainedGlass.Persistence.Services.DB;

internal class DbSanctuaryRegion : DatabasePersistenceService
{
    public override void AddEntity(IPersistanceTransferStruct transferStruct)
    {
        var itemStruct = (SanctuaryRegionDTO)transferStruct;
        var sanctuaryRegion = new SanctuaryRegion
        {
            Name = itemStruct.Name,
            Slug = itemStruct.Slug,
            Description = itemStruct.Description,
            Image = itemStruct.Image,
            SanctuarySideSlug = itemStruct.SanctuarySideSlug
        };
        _dbContext.SanctuaryRegions.Add(sanctuaryRegion);
        _dbContext.SaveChanges();
    }

    public override List<IEntity> GetEntities()
    {
        return new List<IEntity>(_dbContext.SanctuaryRegions);
    }
}