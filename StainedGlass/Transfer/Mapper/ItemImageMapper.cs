using StainedGlass.Persistence.Templates;
using StainedGlass.Persistence.Transfer;

namespace StainedGlass.Transfer.Mapper;

internal class ItemImageMapper : Mapper
{
    public override void SetInstance(IPersistenceTemplate template)
    {
        _persistenceService = template.GetItemImageInstance();
    }

    public override async Task<Transferable?> GetDTOBySlug(string slug)
    {
        throw new NotImplementedException();
    }

    public override async Task<ICollection<Transferable?>> GetAllDTOs()
    {
        throw new NotImplementedException();
    }

    public override async Task<bool> RemoveEntity(string slug)
    {
        return await _persistenceService.RemoveEntity(slug);
    }

    public override async Task<bool> SaveEntity(Transferable transferable)
    {
        throw new NotImplementedException();
    }

    public override async Task<bool> ReplaceEntity(string slug, Transferable transferable)
    {
        throw new NotImplementedException();
    }

    protected override Transferable GetDtoFromTransferable(IPersistanceTransferStruct transferStruct)
    {
        throw new NotImplementedException();
    }
}