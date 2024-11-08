using StainedGlass.Entities;
using StainedGlass.Entities.Transfer;
using StainedGlass.Transfer.Mapper;

namespace StainedGlass.Transfer;

public class UseCaseInteractor : InputBoundary
{
    private Transferable _dataDTO;
    public void SendData(Transferable dataDTO, Mappable mappable)
    {
    
        _dataDTO = dataDTO;
        //TODO send it through the usecases using Chain Of Responsabilities

        //TODO map it to the Entities
        Entity entity = mappable.GetEntity(dataDTO);

        //TODO save entities in DB (sqlite? think about how the tables would be interacted between, OLTP or OLAP, and so on)
        entity.Save();
        throw new NotImplementedException();
    }
}