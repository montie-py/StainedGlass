using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using StainedGlass.Persistence.Entities;
using StainedGlass.Persistence.Transfer;

namespace StainedGlass.Persistence.Services.DB;

internal class DbSanctuarySide : DatabasePersistenceService
{
    public override async Task<bool> AddEntity(IPersistanceTransferStruct transferStruct)
    {
        var itemStruct = (SanctuarySideDTO)GetDtoFromTransfer(transferStruct);
        var sanctuarySide = new SanctuarySide
        {
            Name = itemStruct.Name,
            Slug = itemStruct.Slug,
            ChurchSlug = itemStruct.ChurchSlug
        };
        _dbContext.SanctuarySides.Add(sanctuarySide);
        await _dbContext.SaveChangesAsync();
        return true;
    }

    public override async Task<ICollection<IPersistanceTransferStruct>> GetAllDtos()
    {
        List<IPersistanceTransferStruct> sanctuarySideDtos = new List<IPersistanceTransferStruct>();

        foreach (var sanctuarySideEntity in await _dbContext.SanctuarySides
                     .Include(s => s.Church)
                     .ToListAsync())
        {
            sanctuarySideDtos.Add(GetDtoFromEntity(sanctuarySideEntity));
        }
        
        return sanctuarySideDtos;
    }

    public override async Task<IPersistanceTransferStruct?> GetDtoBySlug(string slug)
    {
        var entity = await _dbContext.SanctuarySides
            .Include(s => s.Church)
            .FirstOrDefaultAsync(s => s.Slug == slug);
        if (entity is null)
        {
            return null;
        }

        return GetDtoFromEntity(entity);
    }
    
    public override async Task<bool> ReplaceEntity(string slug, IPersistanceTransferStruct transferStruct)
    {
        var sanctuarySide = await _dbContext.SanctuarySides.FirstOrDefaultAsync(s => s.Slug == slug);
        if (sanctuarySide != null)
        {
            var transferSanctuarySideDto = (SanctuarySideDTO)GetDtoFromTransfer(transferStruct);
            sanctuarySide.Name = transferSanctuarySideDto.Name;
            sanctuarySide.ChurchSlug = transferSanctuarySideDto.ChurchSlug;
            // _dbContext.SanctuarySides.Update(sanctuarySide);
            // _dbContext.Entry(sanctuarySide).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
        }

        return true;
    }

    public override async Task<bool> RemoveEntity(string slug)
    {
        var sanctuarySide = await _dbContext.SanctuarySides.FirstOrDefaultAsync(x => x.Slug == slug);
        if (sanctuarySide != null)
        {
            _dbContext.SanctuarySides.Remove(sanctuarySide);
            await _dbContext.SaveChangesAsync();
        }

        return true;
    }

    protected override IPersistanceTransferStruct GetDtoFromTransfer(IPersistanceTransferStruct transferStruct)
    {
        return (SanctuarySideDTO)transferStruct;
    }

    protected override IPersistanceTransferStruct GetDtoFromEntity(IEntity entity)
    {
        ChurchDTO churchDto = new();

        var sanctuarySideEntity = (SanctuarySide)entity;

        if (sanctuarySideEntity.Church != null)
        {
            churchDto = new ChurchDTO
            {
                Name = sanctuarySideEntity.Church.Name,
                Slug = sanctuarySideEntity.Church.Slug,
                Description = sanctuarySideEntity.Church.Description,
                Image = new FormFile(sanctuarySideEntity.Church.Image, FileName, fileType)
            };
        }

        return new SanctuarySideDTO
        {
            Name = sanctuarySideEntity.Name,
            Slug = sanctuarySideEntity.Slug,
            ChurchSlug = sanctuarySideEntity.ChurchSlug,
            Church = churchDto
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