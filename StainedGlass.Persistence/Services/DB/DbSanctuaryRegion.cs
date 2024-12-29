using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using StainedGlass.Persistence.Entities;
using StainedGlass.Persistence.Transfer;

namespace StainedGlass.Persistence.Services.DB;

internal class DbSanctuaryRegion : DatabasePersistenceService
{
    public override async Task<bool> AddEntity(IPersistanceTransferStruct transferStruct)
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
        await _dbContext.SaveChangesAsync();
        return true;
    }

    public override async Task<ICollection<IPersistanceTransferStruct>> GetAllDtos()
    {
        List<IPersistanceTransferStruct> sanctuaryRegionDtos = new List<IPersistanceTransferStruct>();

        foreach (var sanctuaryRegion in await _dbContext.SanctuaryRegions
                     .Include(s => s.SanctuarySide)
                     .ToListAsync())
        {
            sanctuaryRegionDtos.Add(GetDtoFromEntity(sanctuaryRegion));
        }
        
        return sanctuaryRegionDtos;
    }

    public override async Task<IPersistanceTransferStruct?> GetDtoBySlug(string slug)
    {
        var entity = await _dbContext.SanctuaryRegions
            .Include(s => s.SanctuarySide)
            .FirstOrDefaultAsync(s => s.Slug == slug);
        if (entity is null)
        {
            return null;
        }

        return GetDtoFromEntity(entity);
    }
    
    public override async Task<bool> ReplaceEntity(string slug, IPersistanceTransferStruct transferStruct)
    {
        var sanctuaryRegion = await _dbContext.SanctuaryRegions.FirstOrDefaultAsync(s => s.Slug == slug);
        if (sanctuaryRegion != null)
        {
            var sanctuaryRegionDTO = (SanctuaryRegionDTO)GetDtoFromTransfer(transferStruct);
            sanctuaryRegion.Name = sanctuaryRegionDTO.Name;
            sanctuaryRegion.Description = sanctuaryRegionDTO.Description;
            sanctuaryRegion.Image = sanctuaryRegionDTO.Image;
            sanctuaryRegion.SanctuarySideSlug = sanctuaryRegionDTO.SanctuarySideSlug;
            // _dbContext.SanctuaryRegions.Update(sanctuaryRegion);
            await _dbContext.SaveChangesAsync();
        }

        return true;
    }
    
    public override async Task<bool> RemoveEntity(string slug)
    {
        var sanctuaryRegion = await _dbContext.SanctuaryRegions.FirstOrDefaultAsync(s => s.Slug == slug);
        if (sanctuaryRegion != null)
        {
            _dbContext.SanctuaryRegions.Remove(sanctuaryRegion);
            await _dbContext.SaveChangesAsync();
        }

        return true;
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
    
    private async Task<byte[]> FormFileToBytes(IFormFile image)
    {
        //transferring file from IFormFile to byte[]
        byte[] filebytes;
        using (var ms = new MemoryStream())
        {
            await image.CopyToAsync(ms);
            filebytes = ms.ToArray();
        }

        return filebytes;
    }
}