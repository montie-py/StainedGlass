using StainedGlass.Persistence.Services;
using StainedGlass.Persistence.Templates;
using StainedGlass.Transfer.Mapper;

public interface Transferable
{
    public Mappable GetMapper();
}