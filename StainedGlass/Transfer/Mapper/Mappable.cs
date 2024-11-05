using StainedGlass.Entities;

namespace StainedGlass.Transfer.Mapper;

public interface Mappable
{
    internal Transferable GetDTO(Entity entity);
    internal Entity GetEntity(Transferable sanctuarySideDTO);
}