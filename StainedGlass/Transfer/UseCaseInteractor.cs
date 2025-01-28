namespace StainedGlass.Transfer;

public class UseCaseInteractor : Persistor, InputBoundary
{
    private Transferable _dataDTO;

    public async Task<bool> StoreEntity(Transferable dataDTO)
    {
        _dataDTO = dataDTO;
        var mapper = dataDTO.GetMapper();
        //todo: send it through the usecases using Chain Of Responsabilities

        mapper.SetInstance(_persistenceTemplate);

        await mapper.SaveEntity(_dataDTO);
        return true;
    }
    
    public async Task<bool> ReplaceEntity(string slug, Transferable dataDTO)
    {
        _dataDTO = dataDTO;
        //TODO send it through the usecases using Chain Of Responsabilities

        //TODO map it to the Entities
        var mapper = dataDTO.GetMapper();
        mapper.SetInstance(_persistenceTemplate);
        return await mapper.ReplaceEntity(slug, _dataDTO);
    }

    public async Task<bool> RemoveEntity<T>(string slug) where T : Transferable, new()
    {
        T entity = new T();
        DTOGeneric<T> dtoGeneric = new(_persistenceTemplate, entity);
        return await dtoGeneric.RemoveEntity(slug);
    }

    public async Task<T?> GetDTOBySlug<T>(string slug, bool includeChildrenToTheResponse = false) where T : Transferable, new()
    {
        T tranferable = new T();
        DTOGeneric<T?> dtoGeneric = new(_persistenceTemplate, tranferable);

        return await dtoGeneric.GetDTOBySlug(slug, includeChildrenToTheResponse);
    }
    
    public async Task<ICollection<T>> GetAllDTOs<T>() where T : Transferable, new()
    {
        T tranferable = new T();
        DTOGeneric<T> dtoGeneric = new(_persistenceTemplate, tranferable);

        return await dtoGeneric.GetAllDTOs();
    }
}