using StainedGlass.Persistence.Templates;
using StainedGlass.Transfer.Mapper;

namespace StainedGlass.Transfer;

public interface InputBoundary
{
     public void StoreEntity(Transferable dataDTO);
     void ReplaceEntity(string slug, Transferable dataDTO);
     public void RemoveEntity<T>(string slug) where T : Transferable, new();
     public T GetDTOBySlug<T>(string slug)  where T : Transferable, new();
     public ICollection<T> GetAllDTOs<T>() where T : Transferable, new();
}