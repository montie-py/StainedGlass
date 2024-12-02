using StainedGlass.Persistence.Entities;
using StainedGlass.Persistence.Transfer;

namespace StainedGlass.Persistence.Services.DB;

internal class DbChurch : DatabasePersistenceService
{
    public override void AddEntity(IPersistanceTransferStruct transferStruct)
    {
        var itemStruct = (ChurchDTO)transferStruct;
        var churchEntity = new Church
        {
            Name = itemStruct.Name,
            Slug = itemStruct.Slug,
            Description = itemStruct.Description,
            Image = itemStruct.Image,
        };
        _dbContext.Churches.Add(churchEntity);
        _dbContext.SaveChanges();
    }

    public override IEnumerable<IPersistanceTransferStruct> GetAllDtos()
    {
        List<IPersistanceTransferStruct> churchDtos = new();
        foreach (var churchEntity in _dbContext.Churches.ToList())
        {
            ChurchDTO churchDto = new ChurchDTO
            {
                Name = churchEntity.Name,
                Description = churchEntity.Description,
                Image = churchEntity.Image,
            };
            
            churchDtos.Add(churchDto);
        }

        return churchDtos;
    }

    public override IPersistanceTransferStruct? GetDto(string slug)
    {
        var entity = _dbContext.Churches.FirstOrDefault(e => e.Slug == slug);
        if (entity is null)
        {
            return null;
        }

        return new ChurchDTO
        {
            Name = entity.Name,
            Description = entity.Description,
            Image = entity.Image,
            Slug = entity.Slug,
        };
    }
}