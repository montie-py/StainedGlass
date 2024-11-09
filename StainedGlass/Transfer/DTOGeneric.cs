using StainedGlass.Entities;
using StainedGlass.Transfer.Mapper;

namespace StainedGlass.Transfer;

internal class DTOGeneric<T> where T : Transferable
{
    private T _here;

    public DTOGeneric(T transferable)
    {
        _here = transferable;
    }

    internal T GetDTOBySlug(string slug)
    {
        Mappable mapper = _here.GetMapper();
        return (T)mapper.GetDTOBySlug(slug);
    }
}