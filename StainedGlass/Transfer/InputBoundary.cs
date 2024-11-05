using StainedGlass.Transfer.Mapper;

namespace StainedGlass.Transfer;

internal interface InputBoundary
{
     public void SendData(Transferable dataDTO, Mappable mappable);
}