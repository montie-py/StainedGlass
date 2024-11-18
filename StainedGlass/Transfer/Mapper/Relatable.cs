using StainedGlass.Entities;

namespace StainedGlass.Transfer.Mapper;

public interface Relatable : Mappable
{
    public Transferable? GetDTO(Entity? entity, bool computeRelatedItems = true);
}