using StainedGlass.Persistence.Entities;
using StainedGlass.Persistence.Transfer;

namespace StainedGlass.Persistence.Services.DB;

internal class DbSanctuarySide : DatabasePersistenceService
{
    public override void AddEntity(IPersistanceTransferStruct transferStruct)
    {
        var itemStruct = (SanctuarySideDTO)transferStruct;
        var sanctuarySide = new SanctuarySide
        {
            Name = itemStruct.Name,
            Slug = itemStruct.Slug,
            ChurchSlug = itemStruct.ChurchSlug
        };
        _dbContext.SanctuarySides.Add(sanctuarySide);
        _dbContext.SaveChanges();
    }

    public override IEnumerable<IPersistanceTransferStruct> GetAllDtos()
    {
        List<IPersistanceTransferStruct> sanctuarySideDtos = new List<IPersistanceTransferStruct>();

        foreach (var sanctuarySide in _dbContext.SanctuarySides.ToList())
        {
            var sanctuarySideDto = new SanctuarySideDTO
            {
                Name = sanctuarySide.Name,
                Slug = sanctuarySide.Slug,
            };
            sanctuarySideDtos.Add(sanctuarySideDto);
        }
        
        return sanctuarySideDtos;
    }
}