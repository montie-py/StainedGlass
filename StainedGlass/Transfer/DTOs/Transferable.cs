using StainedGlass.Entities;
using StainedGlass.Transfer.Mapper;

public interface Transferable
{
    public Entity GetEntity(Transferable transferable);

    public Mappable GetMapper();
}