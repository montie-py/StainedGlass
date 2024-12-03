namespace StainedGlass.Transfer.Mapper;

public interface Relatable : Mappable
{
    public Transferable? GetDTO(Entity? entity, bool skipParentElements = false, bool computeRelatedItems = true);
}