using Microsoft.EntityFrameworkCore;
using StainedGlass.Persistence.Entities;
using StainedGlass.Persistence.Transfer;

namespace StainedGlass.Persistence.Services.DB;

internal class DbItemType : DatabasePersistenceService
{
    public override void AddEntity(IPersistanceTransferStruct transferStruct)
    {
        var itemStruct = (ItemTypeDTO)GetDtoFromTransfer(transferStruct);
        var itemTypeEntity = new ItemType
        {
            Name = itemStruct.Name,
            Slug = itemStruct.Slug,
        };
        _dbContext.ItemTypes.Add(itemTypeEntity);
        _dbContext.SaveChanges();
    }

    public override ICollection<IPersistanceTransferStruct> GetAllDtos()
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
        var entity = _dbContext.ItemTypes
            .Include(it => it.Items)
            .FirstOrDefault(it => it.Slug == slug);
        if (entity is null)
        {
            return null;
        }
        
        //adding items by itemtype
        ICollection<ItemDTO> itemTypeItems = new List<ItemDTO>();

        if (entity.Items != null)
        {
            foreach (var item in entity.Items)
            {
                var itemDto = new ItemDTO
                {
                    Title = item.Title,
                    Slug = item.Slug,
                    Description = item.Description,
                    Image = item.Image,
                };
                itemTypeItems.Add(itemDto);
            }
        }

        return new ItemTypeDTO
        {
            Name = entity.Name,
            Slug = entity.Slug,
            Items = itemTypeItems
        };
    }

    public override void ReplaceEntity(string slug, IPersistanceTransferStruct transferStruct)
    {
        var itemType = _dbContext.ItemTypes.FirstOrDefault(x => x.Slug == slug);

        if (itemType != null)
        {
            var itemStruct = (ItemTypeDTO)GetDtoFromTransfer(transferStruct);
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
        var itemTypeEntity = (ItemType)entity;

        return new ItemTypeDTO
        {
            Name = itemTypeEntity.Name,
            Slug = itemTypeEntity.Slug,
        };
    }
}