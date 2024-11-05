using StainedGlass.Entities;
using StainedGlass.Transfer.Mapper;

namespace StainedGlass.Transfer;

internal class UseCaseInteractor : InputBoundary
{
    private Transferable _dataDTO;
    public void SendData(Transferable dataDTO, Mappable mappable)
    {
        _dataDTO = dataDTO;
        //TODO send it through the usecases using Chain Of Responsabilities

        //TODO map it to the Entities
        Entity entity = mappable.GetEntity(dataDTO);

        //TODO save entities in DB
        throw new NotImplementedException();
    }
}