using Microsoft.EntityFrameworkCore;
using StainedGlass.Persistence.Entities;
using StainedGlass.Persistence.Transfer;

namespace StainedGlass.Persistence.Services.DB;

internal class DbItemImage : DatabasePersistenceService
{
    public override async Task<bool> AddEntity(IPersistanceTransferStruct transferStruct)
    {
        throw new NotImplementedException();
    }

    public override async Task<ICollection<IPersistanceTransferStruct>> GetAllDtos()
    {
        throw new NotImplementedException();
    }

    public override async Task<IPersistanceTransferStruct?> GetDtoBySlug(string slug, bool includeChildrenToTheResponse)
    {
        throw new NotImplementedException();
    }

    public override async Task<bool> RemoveEntity(string slug)
    {
        var itemImage = await _dbContext.ItemImages.FirstOrDefaultAsync(e => e.Slug == slug);
        if (itemImage != null)
        {
            _dbContext.ItemImages.Remove(itemImage);
            return await _dbContext.SaveChangesAsync() > 0;       
        }
        
        return false;
    }

    public override async Task<bool> ReplaceEntity(string slug, IPersistanceTransferStruct transferStruct)
    {
        throw new NotImplementedException();
    }

    protected override IPersistanceTransferStruct GetDtoFromTransfer(IPersistanceTransferStruct transferStruct)
    {
        throw new NotImplementedException();
    }

    protected override IPersistanceTransferStruct GetDtoFromEntity(IEntity entity)
    {
        throw new NotImplementedException();
    }
}