using StainedGlass.Entities;

namespace StainedGlass.Transfer;

public class UseCaseInteractor : InputBoundary
{
    private Transferable _dataDTO;
    public void StoreEntity(Transferable dataDTO)
    {
        _dataDTO = dataDTO;
        //TODO send it through the usecases using Chain Of Responsabilities

        //TODO map it to the Entities
        Entity entity = _dataDTO.GetEntity(dataDTO);

        //TODO save entities in DB (sqlite? think about how the tables would be interacted between, OLTP or OLAP, and so on)
        entity.Save();
    }
    
    public void ReplaceEntity(string slug, Transferable dataDTO)
    {
        _dataDTO = dataDTO;
        //TODO send it through the usecases using Chain Of Responsabilities

        //TODO map it to the Entities
        Entity entity = _dataDTO.GetEntity(dataDTO);
        entity.Replace(slug, entity);
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