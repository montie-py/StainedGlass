namespace StainedGlass.Persistence.Services.Entities;

public class Item
{
    public IEnumerable<Transferable?> GetAllDTOs()
    {
        return EntitiesCollection.Items.Select(e => GetDTO(e.Value));
    }
    
    public Transferable? GetDTOBySlug(string slug)
    {
        if (!EntitiesCollection.Items.ContainsKey(slug))
        {
            return null;
        }
        return GetDTO(EntitiesCollection.Items[slug]);
    }
}