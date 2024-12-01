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

    public override IEnumerable<IPersistanceTransferStruct> GetAllDtos()
    {
        List<IPersistanceTransferStruct> sanctuaryRegionDtos = new List<IPersistanceTransferStruct>();

        foreach (var sanctuaryRegion in _dbContext.SanctuaryRegions.ToList())
        {
            var sanctuaryRegionDto = new SanctuaryRegionDTO
            {
                Name = sanctuaryRegion.Name,
                Slug = sanctuaryRegion.Slug,
                Description = sanctuaryRegion.Description,
                Image = sanctuaryRegion.Image,
            };
            sanctuaryRegionDtos.Add(sanctuaryRegionDto);
        }
        
        return sanctuaryRegionDtos;
    }
}