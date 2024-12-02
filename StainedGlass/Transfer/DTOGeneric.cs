
using StainedGlass.Transfer.Mapper;

namespace StainedGlass.Transfer;

internal class DTOGeneric<T> where T : Transferable
{
    private T _transferable;
    private Mappable mapper;

    public DTOGeneric(T transferable)
    {
        _transferable = transferable;
        mapper = _transferable.GetMapper();
    }

    internal T GetDTOBySlug(string slug)
    {
        return (T)mapper.GetDTOBySlug(slug);
    }

    internal IEnumerable<T> GetAllDTOs()
    {
        IEnumerable<T> result = new List<T>();
        foreach (var dto in mapper.GetAllDTOs())
        {
            ((List<T>)result).Add((T)dto);
        }
        return result;
    }

    public void RemoveEntity(string slug)
    {
        mapper.RemoveEntity(slug);
    }
}