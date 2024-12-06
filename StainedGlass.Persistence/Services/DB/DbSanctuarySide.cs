using StainedGlass.Persistence.Entities;
using StainedGlass.Persistence.Transfer;

namespace StainedGlass.Persistence.Services.DB;

internal class DbSanctuarySide : DatabasePersistenceService
{
    public override void AddEntity(IPersistanceTransferStruct transferStruct)
    {
        var itemStruct = GetSanctuarySideDto(transferStruct);
        var sanctuarySide = new SanctuarySide
        {
            Name = itemStruct.Name,
            Slug = itemStruct.Slug,
            ChurchSlug = itemStruct.ChurchSlug
        };
        _dbContext.SanctuarySides.Add(sanctuarySide);
        _dbContext.SaveChanges();
    }

    public override IEnumerable<IPersistanceTransferStruct> GetAllDtos()
    {
        List<IPersistanceTransferStruct> sanctuarySideDtos = new List<IPersistanceTransferStruct>();

        foreach (var sanctuarySide in _dbContext.SanctuarySides.ToList())
        {
            var sanctuarySideDto = new SanctuarySideDTO
            {
                Name = sanctuarySide.Name,
                Slug = sanctuarySide.Slug,
            };
            sanctuarySideDtos.Add(sanctuarySideDto);
        }
        
        return sanctuarySideDtos;
    }

    public override IPersistanceTransferStruct? GetDtoBySlug(string slug)
    {
        var entity = _dbContext.SanctuarySides.FirstOrDefault(s => s.Slug == slug);
        if (entity is null)
        {
            return null;
        }

        return new SanctuarySideDTO
        {
            Name = entity.Name,
            Slug = entity.Slug,
        };
    }
    
    public override void ReplaceEntity(string slug, IPersistanceTransferStruct transferStruct)
    {
        var sanctuarySide = _dbContext.SanctuarySides.FirstOrDefault(s => s.Slug == slug);
        if (sanctuarySide != null)
        {
            var transferSanctuarySideDto = GetSanctuarySideDto(transferStruct);
            sanctuarySide.Name = transferSanctuarySideDto.Name;
            sanctuarySide.ChurchSlug = transferSanctuarySideDto.ChurchSlug;
            //_dbContext.SanctuarySides.Update(sanctuarySide);
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
    
    private static SanctuarySideDTO GetSanctuarySideDto(IPersistanceTransferStruct transferStruct)
    {
        return (SanctuarySideDTO)transferStruct;
    }
}