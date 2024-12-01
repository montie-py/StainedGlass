using StainedGlass.Persistence.Templates;

namespace StainedGlass.Transfer;

public abstract class Persistor
{
    protected IPersistenceTemplate _persistenceTemplate;
    public void SetPersistenceTemplate(IPersistenceTemplate persistenceTemplate)
    {
        _persistenceTemplate = persistenceTemplate;
    }
}