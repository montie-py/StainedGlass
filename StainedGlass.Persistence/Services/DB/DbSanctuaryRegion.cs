using StainedGlass.Persistence.Entities;
using StainedGlass.Persistence.Transfer;

namespace StainedGlass.Persistence.Services.DB;

internal class DbSanctuaryRegion : DatabasePersistenceService
{
    public override void AddEntity(IPersistanceTransferStruct transferStruct)
    {
        var itemStruct = GetSanctuaryRegionDto(transferStruct);
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

    private static SanctuaryRegionDTO GetSanctuaryRegionDto(IPersistanceTransferStruct transferStruct)
    {
        return (SanctuaryRegionDTO)transferStruct;
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

    public override IPersistanceTransferStruct? GetDtoBySlug(string slug)
    {
        var entity = _dbContext.SanctuaryRegions.FirstOrDefault(s => s.Slug == slug);
        if (entity is null)
        {
            return null;
        }

        return new SanctuaryRegionDTO
        {
            Name = entity.Name,
            Slug = entity.Slug,
            Image = entity.Image,
            Description = entity.Description,
        };
    }
    
    public override void ReplaceEntity(string slug, IPersistanceTransferStruct transferStruct)
    {
        var sanctuaryRegion = _dbContext.SanctuaryRegions.FirstOrDefault(s => s.Slug == slug);
        if (sanctuaryRegion != null)
        {
            var transferSanctuaryRegionDTO = GetSanctuaryRegionDto(transferStruct);
            sanctuaryRegion.Name = transferSanctuaryRegionDTO.Name;
            sanctuaryRegion.Description = transferSanctuaryRegionDTO.Description;
            sanctuaryRegion.Image = transferSanctuaryRegionDTO.Image;
            sanctuaryRegion.SanctuarySideSlug = transferSanctuaryRegionDTO.SanctuarySideSlug;
            // _dbContext.SanctuaryRegions.Update(sanctuaryRegion);
            _dbContext.SaveChanges();
        }
    }

    public override void RemoveEntity(string slug)
    {
        var sanctuaryRegion = _dbContext.SanctuaryRegions.FirstOrDefault(s => s.Slug == slug);
        if (sanctuaryRegion != null)
        {
            _dbContext.SanctuaryRegions.Remove(sanctuaryRegion);
            _dbContext.SaveChanges();
        }
    }
}