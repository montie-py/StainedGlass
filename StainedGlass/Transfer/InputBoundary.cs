using StainedGlass.Transfer.Mapper;

namespace StainedGlass.Transfer;

public interface InputBoundary
{
     public void SendData(Transferable dataDTO, Mappable mappable);
}