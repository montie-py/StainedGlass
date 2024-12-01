
using StainedGlass.Transfer.Mapper;

namespace StainedGlass.Transfer;

internal class DTOGeneric<T> where T : Mappable
{
    private Mappable mapper;

    public DTOGeneric(T mappable)
    {
        mapper = mappable;
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