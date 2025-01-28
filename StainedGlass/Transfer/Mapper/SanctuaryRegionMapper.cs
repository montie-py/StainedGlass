using StainedGlass.Persistence.Templates;
using StainedGlass.Persistence.Transfer;
using SanctuarySideDTO = StainedGlass.Transfer.DTOs.SanctuarySideDTO;
using SanctuaryRegionDTO = StainedGlass.Transfer.DTOs.SanctuaryRegionDTO;

namespace StainedGlass.Transfer.Mapper;

internal class SanctuaryRegionMapper : Mapper
{
    public override void SetInstance(IPersistenceTemplate template)
    {
        _persistenceService = template.GetSanctuaryRegionInstance();
    }
    
    public override async Task<bool> SaveEntity(Transferable transferable)
    {
        Persistence.Transfer.SanctuaryRegionDTO transferSanctuaryRegionDto =
            transferable as SanctuaryRegionDTO;
        return await _persistenceService.AddEntity(transferSanctuaryRegionDto);
    }

    public override async Task<bool> ReplaceEntity(string slug, Transferable transferable)
    {
        Persistence.Transfer.SanctuaryRegionDTO transferSanctuaryRegionDto =
            transferable as SanctuaryRegionDTO;
        return await _persistenceService.ReplaceEntity(slug, transferSanctuaryRegionDto);
    }

    public override async Task<Transferable?> GetDTOBySlug(string slug, bool includeChildrenToTheResponse)
    {
        var nullableTransferSanctuaryRegionDto = await _persistenceService.GetDtoBySlug(slug, includeChildrenToTheResponse);
        if (nullableTransferSanctuaryRegionDto == null)
        {
            return null;
        }

        return GetDtoFromTransferable(nullableTransferSanctuaryRegionDto);
    }
    
    public override async Task<ICollection<Transferable?>> GetAllDTOs()
    {
        var transferSanctuaryRegionDtos = await
            _persistenceService.GetAllDtos();
        var sanctuaryRegionDtos = new List<Transferable>();
        foreach (IPersistanceTransferStruct transferSanctuaryRegionDto in transferSanctuaryRegionDtos)
        {
            sanctuaryRegionDtos.Add(GetDtoFromTransferable(transferSanctuaryRegionDto));
        }

        return sanctuaryRegionDtos;
    }
    
    public override async Task<bool> RemoveEntity(string slug)
    {
        return await _persistenceService.RemoveEntity(slug);
    }
    
    protected override Transferable GetDtoFromTransferable(IPersistanceTransferStruct transferStruct)
    {
        var transferSanctuaryRegionDto = (Persistence.Transfer.SanctuaryRegionDTO)transferStruct;

        SanctuarySideDTO sanctuarySideDto = new();

        if (transferSanctuaryRegionDto.SanctuarySide != null)
        {
            var sanctuarySideTransfer = (Persistence.Transfer.SanctuarySideDTO)transferSanctuaryRegionDto.SanctuarySide;
            sanctuarySideDto = new SanctuarySideDTO
            {
                Name = sanctuarySideTransfer.Name,
                Slug = sanctuarySideTransfer.Slug,
            };
        }

        return new SanctuaryRegionDTO
        {
            Name = transferSanctuaryRegionDto.Name,
            Slug = transferSanctuaryRegionDto.Slug,
            Description = transferSanctuaryRegionDto.Description,
            Image = transferSanctuaryRegionDto.Image,
            SanctuarySideSlug = transferSanctuaryRegionDto.SanctuarySideSlug,
            SanctuarySide = sanctuarySideDto
        };
    }
}