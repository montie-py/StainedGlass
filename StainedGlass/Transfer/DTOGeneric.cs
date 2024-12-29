
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

    internal async Task<T?> GetDTOBySlug(string slug)
    {
        return (T?)await mapper.GetDTOBySlug(slug);
    }

    internal async Task<ICollection<T>> GetAllDTOs()
    {
        ICollection<T> result = new List<T>();
        foreach (var dto in await mapper.GetAllDTOs())
        {
            ((List<T>)result).Add((T)dto);
        }
        return result;
    }

    public async Task<bool> RemoveEntity(string slug)
    {
        return await mapper.RemoveEntity(slug);
    }
}