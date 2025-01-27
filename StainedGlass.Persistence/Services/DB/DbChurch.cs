using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using StainedGlass.Persistence.Entities;
using StainedGlass.Persistence.Transfer;

namespace StainedGlass.Persistence.Services.DB;

internal class DbChurch : DatabasePersistenceService
{
    public DbChurch()
    {
        FileName = "church.jpg";
    }

    public override async Task<bool> AddEntity(IPersistanceTransferStruct transferStruct)
    {
        var churchStruct = (ChurchDTO)GetDtoFromTransfer(transferStruct);
        var churchEntity = new Church
        {
            Name = churchStruct.Name,
            Slug = churchStruct.Slug,
            Description = churchStruct.Description,
        };
        
        //transferring file from IFormFile to byte[]
        churchEntity.Image = await FormFileToBytes(churchStruct.Image);
        
        _dbContext.Churches.Add(churchEntity);
        await _dbContext.SaveChangesAsync();
        return true;
    }

    public override async Task<ICollection<IPersistanceTransferStruct>> GetAllDtos()
    {
        List<IPersistanceTransferStruct> churchDtos = new();
        foreach (var churchEntity in await _dbContext.Churches.ToListAsync())
        {
            churchDtos.Add(GetDtoFromEntity(churchEntity));
        }

        return churchDtos;
    }

    public override async Task<IPersistanceTransferStruct?> GetDtoBySlug(string slug)
    {
        var entity = await _dbContext.Churches
            .Include(c => c.SanctuarySides)
            .FirstOrDefaultAsync(e => e.Slug == slug);
        if (entity is null)
        {
            return null;
        }

        return GetDtoFromEntity(entity);
    }

    public override async Task<bool> ReplaceEntity(string slug, IPersistanceTransferStruct transferStruct)
    {
        var churchEntity = await _dbContext.Churches.FirstOrDefaultAsync(e => e.Slug == slug);
        if (churchEntity != null)
        {
            var churchStruct = (ChurchDTO)GetDtoFromTransfer(transferStruct);
            churchEntity.Name = churchStruct.Name;
            churchEntity.Description = churchStruct.Description;

            if (churchStruct.Image != null)
            {
                //transferring file from IFormFile to byte[]
                churchEntity.Image = await FormFileToBytes(churchStruct.Image);
            }
            
            // _dbContext.Churches.Update(church);
            await _dbContext.SaveChangesAsync();
        }

        return true;
    }

    public override async Task<bool> RemoveEntity(string slug)
    {
        var church = await _dbContext.Churches.FirstOrDefaultAsync(e => e.Slug == slug);
        if (church != null)
        {
            _dbContext.Churches.Remove(church);
            await _dbContext.SaveChangesAsync();
        }

        return true;
    }
    
    protected override IPersistanceTransferStruct GetDtoFromTransfer(IPersistanceTransferStruct transferStruct)
    {
        return (ChurchDTO)transferStruct;
    }

    protected override IPersistanceTransferStruct GetDtoFromEntity(IEntity entity)
    {
        var churchEntity = (Church)entity;

        var sanctuarySideDTOs = new HashSet<SanctuarySideDTO>();

        if (churchEntity.SanctuarySides != null && churchEntity.SanctuarySides.Count > 0)
        {
            foreach (var sanctuarySide in churchEntity.SanctuarySides)
            {
                sanctuarySideDTOs.Add(new SanctuarySideDTO
                {
                    Name = sanctuarySide.Name,
                    Slug = sanctuarySide.Slug,
                    Position = sanctuarySide.Position
                });
            }
        }

    return new ChurchDTO
        {
            Name = churchEntity.Name,
            Description = churchEntity.Description,
            Image = new FormFile(churchEntity.Image, FileName, fileType),
            Slug = churchEntity.Slug,
            Sides = sanctuarySideDTOs
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