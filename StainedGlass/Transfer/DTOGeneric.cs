
using StainedGlass.Persistence.Templates;
using StainedGlass.Transfer.Mapper;

namespace StainedGlass.Transfer;

internal class DTOGeneric<T> where T : Transferable
{
    private T _transferable;
    private Mappable mapper;

    public DTOGeneric(IPersistenceTemplate persistenceTemplate, T transferable)
    {
        _transferable = transferable;
        mapper = _transferable.GetMapper();
        mapper.SetInstance(persistenceTemplate);
    }

    internal T GetDTOBySlug(string slug)
    {
        return (T)mapper.GetDTOBySlug(slug);
    }

    internal ICollection<T> GetAllDTOs()
    {
        ICollection<T> result = new List<T>();
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