using StainedGlass.Persistence.Templates;
using StainedGlass.Transfer.Mapper;

namespace StainedGlass.Transfer;

public interface InputBoundary
{
     public Task<bool> StoreEntity(Transferable dataDTO);
     public Task<bool> ReplaceEntity(string slug, Transferable dataDTO);
     public Task<bool> RemoveEntity<T>(string slug) where T : Transferable, new();
     public Task<T?> GetDTOBySlug<T>(string slug)  where T : Transferable, new();
     public Task<ICollection<T>> GetAllDTOs<T>() where T : Transferable, new();
}