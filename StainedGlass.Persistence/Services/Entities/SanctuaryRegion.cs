namespace StainedGlass.Persistence.Services.Entities;

public class SanctuaryRegion
{
    public IEnumerable<Transferable?> GetAllDTOs()
    {
        return EntitiesCollection.SanctuaryRegions.Select(
            e => GetDTO(e.Value, skipParentElements: true)
        );
    }
    
    public Transferable? GetDTOBySlug(string slug)
    {
        if (!EntitiesCollection.SanctuaryRegions.ContainsKey(slug))
        {
            return null;
        }
        return GetDTO(EntitiesCollection.SanctuaryRegions[slug], skipParentElements: true);
    }
}