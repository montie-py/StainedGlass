using StainedGlass.Persistence.Services;
using StainedGlass.Persistence.Services.Entities;

namespace StainedGlass.Persistence.Templates;

public class EntitiesPersistenceTemplate : IPersistenceTemplate
{
    public IPersistenceService GetItemInstance()
    {
        return new EntityItem();
    }

    public IPersistenceService GetItemTypeInstance()
    {
        return new EntityItemType();
    }

    public IPersistenceService GetSanctuaryRegionInstance()
    {
        return new EntitySanctuaryRegion();
    }

    public IPersistenceService GetSanctuarySideInstance()
    {
        return new EntitySanctuarySide();
    }

    public IPersistenceService GetChurchInstance()
    {
        return new EntityChurch();
    }
}