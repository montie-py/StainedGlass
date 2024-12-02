using StainedGlass.Persistence.Entities;
using StainedGlass.Persistence.Transfer;

namespace StainedGlass.Persistence.Services.DB;

internal class DbItemType : DatabasePersistenceService
{
    public override void AddEntity(IPersistanceTransferStruct transferStruct)
    {
        var itemStruct = (ItemTypeDTO)transferStruct;
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

    public override IPersistanceTransferStruct? GetDto(string slug)
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
}