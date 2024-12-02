namespace StainedGlass.Persistence.Services.Entities;

public class SanctuarySide
{
    public IEnumerable<Transferable?> GetAllDTOs()
    {
        return EntitiesCollection.SanctuarySides.Select(
            e => GetDTO(e.Value, skipParentElements: true)
        );
    }
    
    public Transferable? GetDTOBySlug(string slug)
    {
        if (!EntitiesCollection.SanctuarySides.ContainsKey(slug))
        {
            return null;
        }
        return GetDTO(EntitiesCollection.SanctuarySides[slug], skipParentElements: true);
    }
}