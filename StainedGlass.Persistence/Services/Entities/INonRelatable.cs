using StainedGlass.Persistence.Transfer;

namespace StainedGlass.Persistence.Services.Entities;

public interface INonRelatable 
{
    public IPersistanceTransferStruct? GetDTO(Entity? entity, bool skipParentElements = false, bool skipChildrenElements = false);
}