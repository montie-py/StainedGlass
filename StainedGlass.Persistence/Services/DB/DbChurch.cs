using StainedGlass.Persistence.Entities;
using StainedGlass.Persistence.Transfer;

namespace StainedGlass.Persistence.Services.DB;

internal class DbChurch : DatabasePersistenceService
{
    public override void AddEntity(IPersistanceTransferStruct transferStruct)
    {
        var churchStruct = (ChurchDTO)GetDtoFromTransfer(transferStruct);
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

    public override ICollection<IPersistanceTransferStruct> GetAllDtos()
    {
        var here = _dbContext.Churches.ToList();
        List<IPersistanceTransferStruct> churchDtos = new();
        foreach (var churchEntity in _dbContext.Churches.ToList())
        {
            churchDtos.Add(GetDtoFromEntity(churchEntity));
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

        return GetDtoFromEntity(entity);
    }

    public override void ReplaceEntity(string slug, IPersistanceTransferStruct transferStruct)
    {
        var church = _dbContext.Churches.FirstOrDefault(e => e.Slug == slug);
        if (church != null)
        {
            var churchStruct = (ChurchDTO)GetDtoFromTransfer(transferStruct);
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
    
    protected override IPersistanceTransferStruct GetDtoFromTransfer(IPersistanceTransferStruct transferStruct)
    {
        return (ChurchDTO)transferStruct;
    }

    protected override IPersistanceTransferStruct GetDtoFromEntity(IEntity entity)
    {
        var churchEntity = (Church)entity;
        return new ChurchDTO
        {
            Name = churchEntity.Name,
            Description = churchEntity.Description,
            Image = churchEntity.Image,
            Slug = churchEntity.Slug,
        };
    }
}