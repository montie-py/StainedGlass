using StainedGlass.Persistence.Entities;
using StainedGlass.Persistence.Transfer;

namespace StainedGlass.Persistence.Services.Entities;

public class EntityItemImage: INonRelatable, IPersistenceService
{
    public IEntity GetEntity(IPersistanceTransferStruct transferable)
    {
        throw new NotImplementedException();
    }

    public IPersistanceTransferStruct? GetDTOForEntity(IEntity? entity, bool skipParentElements = false,
        bool skipChildrenElements = false)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> AddEntity(IPersistanceTransferStruct transferStruct)
    {
        throw new NotImplementedException();
    }

    public async Task<ICollection<IPersistanceTransferStruct>> GetAllDtos()
    {
        throw new NotImplementedException();
    }

    public async Task<IPersistanceTransferStruct?> GetDtoBySlug(string slug, bool includeChildrenToTheResponse)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> RemoveEntity(string slug)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> ReplaceEntity(string slug, IPersistanceTransferStruct transferStruct)
    {
        throw new NotImplementedException();
    }
}