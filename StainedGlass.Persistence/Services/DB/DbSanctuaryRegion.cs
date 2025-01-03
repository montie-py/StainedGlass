using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using StainedGlass.Persistence.Entities;
using StainedGlass.Persistence.Transfer;

namespace StainedGlass.Persistence.Services.DB;

internal class DbSanctuaryRegion : DatabasePersistenceService
{
    public override async Task<bool> AddEntity(IPersistanceTransferStruct transferStruct)
    {
        var sanctuaryRegionStruct = (SanctuaryRegionDTO)GetDtoFromTransfer(transferStruct);
        var sanctuaryRegionEntity = new SanctuaryRegion
        {
            Name = sanctuaryRegionStruct.Name,
            Slug = sanctuaryRegionStruct.Slug,
            Description = sanctuaryRegionStruct.Description,
            SanctuarySideSlug = sanctuaryRegionStruct.SanctuarySideSlug
        };
        
        //transferring file from IFormFile to byte[]
        sanctuaryRegionEntity.Image = await FormFileToBytes(sanctuaryRegionStruct.Image);
        
        _dbContext.SanctuaryRegions.Add(sanctuaryRegionEntity);
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
        var sanctuaryRegionEntity = await _dbContext.SanctuaryRegions.FirstOrDefaultAsync(s => s.Slug == slug);
        if (sanctuaryRegionEntity != null)
        {
            var sanctuaryRegionStruct = (SanctuaryRegionDTO)GetDtoFromTransfer(transferStruct);
            sanctuaryRegionEntity.Name = sanctuaryRegionStruct.Name;
            sanctuaryRegionEntity.Description = sanctuaryRegionStruct.Description;
            sanctuaryRegionEntity.SanctuarySideSlug = sanctuaryRegionStruct.SanctuarySideSlug;

            if (sanctuaryRegionStruct.Image != null)
            {
                //transferring file from IFormFile to byte[]
                sanctuaryRegionEntity.Image = await FormFileToBytes(sanctuaryRegionStruct.Image);
            }
            
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
            Image = new FormFile(sanctuaryRegionEntity.Image, FileName, fileType),
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