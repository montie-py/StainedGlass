using StainedGlass.Persistence.Entities;
using StainedGlass.Persistence.Transfer;

namespace StainedGlass.Persistence.Services.DB;

internal class DbItemType : DatabasePersistenceService
{
    public override void AddEntity(IPersistanceTransferStruct transferStruct)
    {
        var itemStruct = (ItemType)GetDtoFromTransfer(transferStruct);
        var itemTypeEntity = new ItemType
        {
            Name = itemStruct.Name,
            Slug = itemStruct.Slug,
        };
        _dbContext.ItemTypes.Add(itemTypeEntity);
        _dbContext.SaveChanges();
    }

    public override IEnumerable<IPersistanceTransferStruct> GetAllDtos()
    {
        List<IPersistanceTransferStruct> itemTypeDtos = new List<IPersistanceTransferStruct>();
        foreach (var itemType in _dbContext.ItemTypes.ToList())
        {
            var itemTypeDto = new ItemTypeDTO
            {
                Name = itemType.Name,
                Slug = itemType.Slug,
            };
            itemTypeDtos.Add(itemTypeDto);
        }
        return itemTypeDtos;
    }

    public override IPersistanceTransferStruct? GetDtoBySlug(string slug)
    {
        var entity = _dbContext.ItemTypes.FirstOrDefault(x => x.Name == slug);
        if (entity is null)
        {
            return null;
        }

        return new ItemTypeDTO
        {
            Name = entity.Name,
            Slug = entity.Slug,
        };
    }

    public override void ReplaceEntity(string slug, IPersistanceTransferStruct transferStruct)
    {
        var itemType = _dbContext.ItemTypes.FirstOrDefault(x => x.Slug == slug);

        if (itemType != null)
        {
            var itemStruct = (ItemType)GetDtoFromTransfer(transferStruct);
            itemType.Name = itemStruct.Name;
            _dbContext.SaveChanges();
        }
    }

    public override void RemoveEntity(string slug)
    {
        var itemType = _dbContext.ItemTypes.FirstOrDefault(x => x.Slug == slug);
        if (itemType != null)
        {
            _dbContext.ItemTypes.Remove(itemType);
            _dbContext.SaveChanges();
        }
    }
    
    protected override IPersistanceTransferStruct GetDtoFromTransfer(IPersistanceTransferStruct transferStruct)
    {
        return (ItemTypeDTO)transferStruct;
    }

    protected override IPersistanceTransferStruct GetDtoFromEntity(IEntity entity)
    {
        throw new NotImplementedException();
    }
}