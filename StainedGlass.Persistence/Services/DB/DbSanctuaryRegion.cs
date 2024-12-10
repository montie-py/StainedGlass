using StainedGlass.Persistence.Entities;
using StainedGlass.Persistence.Transfer;

namespace StainedGlass.Persistence.Services.DB;

internal class DbSanctuaryRegion : DatabasePersistenceService
{
    public override void AddEntity(IPersistanceTransferStruct transferStruct)
    {
        var sanctuaryRegionDto = (SanctuaryRegionDTO)GetDtoFromTransfer(transferStruct);
        var sanctuaryRegion = new SanctuaryRegion
        {
            Name = sanctuaryRegionDto.Name,
            Slug = sanctuaryRegionDto.Slug,
            Description = sanctuaryRegionDto.Description,
            Image = sanctuaryRegionDto.Image,
            SanctuarySideSlug = sanctuaryRegionDto.SanctuarySideSlug
        };
        _dbContext.SanctuaryRegions.Add(sanctuaryRegion);
        _dbContext.SaveChanges();
    }

    public override IEnumerable<IPersistanceTransferStruct> GetAllDtos()
    {
        List<IPersistanceTransferStruct> sanctuaryRegionDtos = new List<IPersistanceTransferStruct>();

        foreach (var sanctuaryRegion in _dbContext.SanctuaryRegions.ToList())
        {
            sanctuaryRegionDtos.Add(GetDtoFromEntity(sanctuaryRegion));
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

        return GetDtoFromEntity(entity);
    }
    
    public override void ReplaceEntity(string slug, IPersistanceTransferStruct transferStruct)
    {
        var sanctuaryRegion = _dbContext.SanctuaryRegions.FirstOrDefault(s => s.Slug == slug);
        if (sanctuaryRegion != null)
        {
            var sanctuaryRegionDTO = (SanctuaryRegionDTO)GetDtoFromTransfer(transferStruct);
            sanctuaryRegion.Name = sanctuaryRegionDTO.Name;
            sanctuaryRegion.Description = sanctuaryRegionDTO.Description;
            sanctuaryRegion.Image = sanctuaryRegionDTO.Image;
            sanctuaryRegion.SanctuarySideSlug = sanctuaryRegionDTO.SanctuarySideSlug;
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
    
    protected override IPersistanceTransferStruct GetDtoFromTransfer(IPersistanceTransferStruct transferStruct)
    {
        return (SanctuaryRegionDTO)transferStruct;
    }

    protected override IPersistanceTransferStruct GetDtoFromEntity(IEntity entity)
    {
        var sanctuaryRegionEntity = (SanctuaryRegion)entity;
        SanctuarySideDTO sanctuarySideDto = new();

        if (sanctuaryRegionEntity.SanctuarySide != null)
        {
            sanctuarySideDto = new SanctuarySideDTO
            {
                Name = sanctuaryRegionEntity.SanctuarySide.Name,
                Slug = sanctuaryRegionEntity.SanctuarySide.Slug,
            };
        }

        return new SanctuaryRegionDTO
        {
            Name = sanctuaryRegionEntity.Name,
            Slug = sanctuaryRegionEntity.Slug,
            Image = sanctuaryRegionEntity.Image,
            Description = sanctuaryRegionEntity.Description,
            SanctuarySideSlug = sanctuaryRegionEntity.SanctuarySideSlug,
            SanctuarySide = sanctuarySideDto
        };
    }
}