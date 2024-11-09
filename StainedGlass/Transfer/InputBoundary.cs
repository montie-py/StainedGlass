using StainedGlass.Transfer.Mapper;

namespace StainedGlass.Transfer;

public interface InputBoundary
{
     public void StoreEntity(Transferable dataDTO);
     public T GetDTOBySlug<T>(string slug)  where T : Transferable, new();
}