using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using StainedGlass.Persistence.Entities;
using StainedGlass.Persistence.Transfer;

namespace StainedGlass.Persistence.Services.DB;

internal class DbItemType : DatabasePersistenceService
{
    public override async Task<bool> AddEntity(IPersistanceTransferStruct transferStruct)
    {
        var itemStruct = (ItemTypeDTO)GetDtoFromTransfer(transferStruct);
        var itemTypeEntity = new ItemType
        {
            Name = itemStruct.Name,
            Slug = itemStruct.Slug,
        };
        _dbContext.ItemTypes.Add(itemTypeEntity);
        await _dbContext.SaveChangesAsync();
        return true;
    }

    public override async Task<ICollection<IPersistanceTransferStruct>> GetAllDtos()
    {
        List<IPersistanceTransferStruct> itemTypeDtos = new List<IPersistanceTransferStruct>();
        foreach (var itemType in await _dbContext.ItemTypes.ToListAsync())
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

    public override async Task<IPersistanceTransferStruct?> GetDtoBySlug(string slug)
    {
        var entity = await _dbContext.ItemTypes
            .Include(it => it.Items)
            .FirstOrDefaultAsync(it => it.Slug == slug);
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

    public override async Task<bool> ReplaceEntity(string slug, IPersistanceTransferStruct transferStruct)
    {
        var itemType = await _dbContext.ItemTypes.FirstOrDefaultAsync(x => x.Slug == slug);

        if (itemType != null)
        {
            var itemStruct = (ItemTypeDTO)GetDtoFromTransfer(transferStruct);
            itemType.Name = itemStruct.Name;
            await _dbContext.SaveChangesAsync();
        }

        return true;
    }

    public override async Task<bool> RemoveEntity(string slug)
    {
        var itemType = await _dbContext.ItemTypes.FirstOrDefaultAsync(x => x.Slug == slug);
        if (itemType != null)
        {
            _dbContext.ItemTypes.Remove(itemType);
            await _dbContext.SaveChangesAsync();
        }

        return true;
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