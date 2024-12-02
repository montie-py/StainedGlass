namespace StainedGlass.Persistence.Services.Entities;

public class ItemType
{
    public IEnumerable<Transferable?> GetAllDTOs()
    {
        return EntitiesCollection.ItemsTypes.Select(e => GetDTO(e.Value)).ToList();
    }
    
    public Transferable? GetDTOBySlug(string slug)
    {
        if (!EntitiesCollection.ItemsTypes.ContainsKey(slug))
        {
            return null;
        }
        return GetDTO(EntitiesCollection.ItemsTypes[slug]);
    }
}