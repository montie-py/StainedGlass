using StainedGlass.Persistence.Entities;
using StainedGlass.Persistence.Services.Entities;
using StainedGlass.Persistence.Transfer;

namespace StainedGlass.Transfer.Mapper;

public interface IRelatable : IInMemoryService
{
    public IPersistanceTransferStruct? GetDTOForEntity(
        IEntity? entity, 
        bool skipParentElements = false, 
        bool computeRelatedItems = true
        );
}