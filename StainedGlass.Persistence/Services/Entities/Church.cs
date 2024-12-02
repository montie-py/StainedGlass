namespace StainedGlass.Persistence.Services.Entities;

public class Church
{
    public IEnumerable<Transferable?> GetAllDTOs()
    {
        return EntitiesCollection.Churches.Select(e => GetDTO(e.Value));
    }
    
    public Transferable? GetDTOBySlug(string slug)
    {
        if (!EntitiesCollection.Churches.ContainsKey(slug))
        {
            return null;
        }
        return GetDTO(EntitiesCollection.Churches[slug]);
    }
}