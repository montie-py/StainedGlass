using Microsoft.EntityFrameworkCore;
using StainedGlass.Persistence.Entities;
using StainedGlass.Persistence.Transfer;

namespace StainedGlass.Persistence.Services.DB;

internal class DbSanctuarySide : DatabasePersistenceService
{
    public override void AddEntity(IPersistanceTransferStruct transferStruct)
    {
        var itemStruct = (SanctuarySideDTO)GetDtoFromTransfer(transferStruct);
        var sanctuarySide = new SanctuarySide
        {
            Name = itemStruct.Name,
            Slug = itemStruct.Slug,
            ChurchSlug = itemStruct.ChurchSlug
        };
        _dbContext.SanctuarySides.Add(sanctuarySide);
        _dbContext.SaveChanges();
    }

    public override ICollection<IPersistanceTransferStruct> GetAllDtos()
    {
        List<IPersistanceTransferStruct> sanctuarySideDtos = new List<IPersistanceTransferStruct>();

        foreach (var sanctuarySideEntity in _dbContext.SanctuarySides
                     .Include(s => s.Church)
                     .ToList())
        {
            sanctuarySideDtos.Add(GetDtoFromEntity(sanctuarySideEntity));
        }
        
        return sanctuarySideDtos;
    }

    public override IPersistanceTransferStruct? GetDtoBySlug(string slug)
    {
        var entity = _dbContext.SanctuarySides
            .Include(s => s.Church)
            .FirstOrDefault(s => s.Slug == slug);
        if (entity is null)
        {
            return null;
        }

        return GetDtoFromEntity(entity);
    }
    
    public override void ReplaceEntity(string slug, IPersistanceTransferStruct transferStruct)
    {
        var sanctuarySide = _dbContext.SanctuarySides.FirstOrDefault(s => s.Slug == slug);
        if (sanctuarySide != null)
        {
            var transferSanctuarySideDto = (SanctuarySideDTO)GetDtoFromTransfer(transferStruct);
            sanctuarySide.Name = transferSanctuarySideDto.Name;
            sanctuarySide.ChurchSlug = transferSanctuarySideDto.ChurchSlug;
            // _dbContext.SanctuarySides.Update(sanctuarySide);
            // _dbContext.Entry(sanctuarySide).State = EntityState.Modified;
            _dbContext.SaveChanges();
        }
    }

    public override void RemoveEntity(string slug)
    {
        var sanctuarySide = _dbContext.SanctuarySides.FirstOrDefault(x => x.Slug == slug);
        if (sanctuarySide != null)
        {
            _dbContext.SanctuarySides.Remove(sanctuarySide);
            _dbContext.SaveChanges();
        }
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
                // Image = sanctuarySideEntity.Church.Image,
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
}