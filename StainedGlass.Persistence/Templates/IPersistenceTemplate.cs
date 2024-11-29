using StainedGlass.Persistence.Services;

namespace StainedGlass.Persistence.Templates;

public interface IPersistenceTemplate
{
    public IPersistenceService GetItemInstance();
    public IPersistenceService GetItemTypeInstance();
    public IPersistenceService GetSanctuaryRegionInstance();
    public IPersistenceService GetSanctuarySideInstance();
    public IPersistenceService GetChurchInstance();
}