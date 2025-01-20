using StainedGlass.Persistence.Services;
using StainedGlass.Persistence.Services.DB;

namespace StainedGlass.Persistence.Templates;

public class DatabasePersistenceTemplate : IPersistenceTemplate
{
    public IPersistenceService GetItemInstance()
    {
        return new DbItem();
    }

    public IPersistenceService GetItemTypeInstance()
    {
        return new DbItemType();
    }

    public IPersistenceService GetSanctuaryRegionInstance()
    {
        return new DbSanctuaryRegion();
    }

    public IPersistenceService GetSanctuarySideInstance()
    {
        return new DbSanctuarySide();
    }

    public IPersistenceService GetChurchInstance()
    {
        return new DbChurch();
    }

    public IPersistenceService GetItemImageInstance()
    {
        return new DbItemImage();
    }
}