using StainedGlass.Persistence.Templates;
using StainedGlass.Persistence.Transfer;
using ChurchDTO = StainedGlass.Transfer.DTOs.ChurchDTO;
using SanctuarySideDTO = StainedGlass.Transfer.DTOs.SanctuarySideDTO;

namespace StainedGlass.Transfer.Mapper;

internal class SanctuarySideMapper : Mapper
{    
    public override void SetInstance(IPersistenceTemplate template)
    {
        _persistenceService = template.GetSanctuarySideInstance();
    }
    
    public override async Task<bool> SaveEntity(Transferable transferable)
    {
        Persistence.Transfer.SanctuarySideDTO transferSanctuarySideDTO = 
            transferable as SanctuarySideDTO;
        return await _persistenceService.AddEntity(transferSanctuarySideDTO);
    }

    public override async Task<bool> ReplaceEntity(string slug, Transferable transferable)
    {
        Persistence.Transfer.SanctuarySideDTO transferSanctuarySideDTO = 
            transferable as SanctuarySideDTO;
        return await _persistenceService.ReplaceEntity(slug, transferSanctuarySideDTO);
    }

    public override async Task<Transferable?> GetDTOBySlug(string slug)
    {
        var nullableTransferSanctuarySideDto = await _persistenceService.GetDtoBySlug(slug);
        if (nullableTransferSanctuarySideDto is null)
        {
            return null;
        }

        return GetDtoFromTransferable(nullableTransferSanctuarySideDto);
    }
    
    public override async Task<ICollection<Transferable?>> GetAllDTOs()
    {
        var transferSanctuarySideDtos = await
            _persistenceService.GetAllDtos();
        var sanctuarySideDtos = new List<Transferable>();
        foreach (Persistence.Transfer.SanctuarySideDTO transferSanctuarySideDto in transferSanctuarySideDtos)
        {
            sanctuarySideDtos.Add(GetDtoFromTransferable(transferSanctuarySideDto));
        }
        return sanctuarySideDtos;
    }
    
    public override async Task<bool> RemoveEntity(string slug)
    {
        return await _persistenceService.RemoveEntity(slug);
    }

    protected override Transferable GetDtoFromTransferable(IPersistanceTransferStruct transferStruct)
    {
        var transferSanctuarySideDto = (Persistence.Transfer.SanctuarySideDTO)transferStruct;
        ChurchDTO churchDto = new();

        if (transferSanctuarySideDto.Church != null)
        {
            var church = (Persistence.Transfer.ChurchDTO)transferSanctuarySideDto.Church;
            churchDto = new ChurchDTO
            {
                Name = church.Name,
                Slug = church.Slug,
                Description = church.Description,
                Image = church.Image
            };
        }

        return new SanctuarySideDTO
        {
            Name = transferSanctuarySideDto.Name,
            Slug = transferSanctuarySideDto.Slug,
            Position = transferSanctuarySideDto.Position,
            ChurchSlug = transferSanctuarySideDto.ChurchSlug,
            Church = churchDto
        };
    }
}