namespace StainedGlass.Transfer;

public class UseCaseInteractor : Persistor, InputBoundary
{
    private Transferable _dataDTO;

    public void StoreEntity(Transferable dataDTO)
    {
        _dataDTO = dataDTO;
        var mapper = dataDTO.GetMapper();
        //todo: send it through the usecases using Chain Of Responsabilities

        mapper.SetInstance(_persistenceTemplate);

        mapper.SaveEntity(_dataDTO);
    }
    
    public void ReplaceEntity(string slug, Transferable dataDTO)
    {
        _dataDTO = dataDTO;
        //TODO send it through the usecases using Chain Of Responsabilities

        //TODO map it to the Entities
        var mapper = dataDTO.GetMapper();
        mapper.SetInstance(_persistenceTemplate);
        mapper.ReplaceEntity(slug, _dataDTO);
    }

    public void RemoveEntity<T>(string slug) where T : Transferable, new()
    {
        T entity = new T();
        DTOGeneric<T> dtoGeneric = new(entity);
        dtoGeneric.RemoveEntity(slug);
    }

    public T GetDTOBySlug<T>(string slug) where T : Transferable, new()
    {
        T tranferable = new T();
        DTOGeneric<T> dtoGeneric = new(tranferable);

        return dtoGeneric.GetDTOBySlug(slug);
    }
    
    public IEnumerable<T> GetAllDTOs<T>() where T : Transferable, new()
    {
        T tranferable = new T();
        DTOGeneric<T> dtoGeneric = new(tranferable);

        return dtoGeneric.GetAllDTOs();
    }
}