using StainedGlass.Persistence.Templates;
using StainedGlass.Transfer.DTOs;

namespace StainedGlass.Transfer.Mapper;

internal class SanctuarySideMapper : Mapper
{    
    public override void SetInstance(IPersistenceTemplate template)
    {
        _persistenceService = template.GetSanctuarySideInstance();
    }
    
    public override void SaveEntity(Transferable transferable)
    {
        Persistence.Transfer.SanctuarySideDTO transferSanctuarySideDTO = 
            transferable as SanctuarySideDTO;
        _persistenceService.AddEntity(transferSanctuarySideDTO);
    }

    public override void ReplaceEntity(string slug, Transferable transferable)
    {
        Persistence.Transfer.SanctuarySideDTO transferSanctuarySideDTO = 
            transferable as SanctuarySideDTO;
        _persistenceService.ReplaceEntity(slug, transferSanctuarySideDTO);
    }

    public override Transferable? GetDTOBySlug(string slug)
    {
        var nullableTransferSanctuarySideDto = _persistenceService.GetDtoBySlug(slug);
        if (nullableTransferSanctuarySideDto is null)
        {
            return null;
        }

        var transferSanctuarySideDto = (Persistence.Transfer.SanctuarySideDTO)nullableTransferSanctuarySideDto;

        return new SanctuarySideDTO
        {
            Name = transferSanctuarySideDto.Name,
            Slug = transferSanctuarySideDto.Slug,
        };
    }
    
    public override IEnumerable<Transferable?> GetAllDTOs()
    {
        var transferSanctuarySideDtos = 
            _persistenceService.GetAllDtos() as IEnumerable<Persistence.Transfer.SanctuarySideDTO>;
        var sanctuarySideDtos = new List<SanctuarySideDTO>();
        foreach (var transferSanctuarySideDto in transferSanctuarySideDtos)
        {
            var sanctuarySideDto = new SanctuarySideDTO
            {
                Name = transferSanctuarySideDto.Name,
                Slug = transferSanctuarySideDto.Slug,
            };
            sanctuarySideDtos.Add(sanctuarySideDto);
        }
        return sanctuarySideDtos;
    }
    
    public override void RemoveEntity(string slug)
    {
        _persistenceService.RemoveEntity(slug);
    }
}