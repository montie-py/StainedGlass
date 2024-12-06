using StainedGlass.Persistence.Entities;
using StainedGlass.Persistence.Transfer;

namespace StainedGlass.Persistence.Services.DB;

internal class DbChurch : DatabasePersistenceService
{
    public override void AddEntity(IPersistanceTransferStruct transferStruct)
    {
        var churchStruct = GetChurchDto(transferStruct);
        var churchEntity = new Church
        {
            Name = churchStruct.Name,
            Slug = churchStruct.Slug,
            Description = churchStruct.Description,
            Image = churchStruct.Image,
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

    public override IPersistanceTransferStruct? GetDtoBySlug(string slug)
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

    public override void ReplaceEntity(string slug, IPersistanceTransferStruct transferStruct)
    {
        var church = _dbContext.Churches.FirstOrDefault(e => e.Slug == slug);
        if (church != null)
        {
            var churchStruct = GetChurchDto(transferStruct);
            church.Name = churchStruct.Name;
            church.Description = churchStruct.Description;
            church.Image = churchStruct.Image;
            // _dbContext.Churches.Update(church);
            _dbContext.SaveChanges();
        }
    }
    
    public override void RemoveEntity(string slug)
    {
        var church = _dbContext.Churches.FirstOrDefault(e => e.Slug == slug);
        if (church != null)
        {
            _dbContext.Churches.Remove(church);
            _dbContext.SaveChanges();
        }
    }

    private ChurchDTO GetChurchDto(IPersistanceTransferStruct transferStruct)
    {
        return (ChurchDTO)transferStruct;
    }
}