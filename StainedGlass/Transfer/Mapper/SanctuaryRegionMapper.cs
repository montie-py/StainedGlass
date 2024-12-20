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
    
    public override void SaveEntity(Transferable transferable)
    {
        Persistence.Transfer.SanctuaryRegionDTO transferSanctuaryRegionDto =
            transferable as SanctuaryRegionDTO;
        _persistenceService.AddEntity(transferSanctuaryRegionDto);
    }

    public override void ReplaceEntity(string slug, Transferable transferable)
    {
        Persistence.Transfer.SanctuaryRegionDTO transferSanctuaryRegionDto =
            transferable as SanctuaryRegionDTO;
        _persistenceService.ReplaceEntity(slug, transferSanctuaryRegionDto);
    }

    public override Transferable? GetDTOBySlug(string slug)
    {
        var nullableTransferSanctuaryRegionDto = _persistenceService.GetDtoBySlug(slug);
        if (nullableTransferSanctuaryRegionDto == null)
        {
            return null;
        }

        return GetDtoFromTransferable(nullableTransferSanctuaryRegionDto);
    }
    
    public override ICollection<Transferable?> GetAllDTOs()
    {
        var transferSanctuaryRegionDtos =
            _persistenceService.GetAllDtos();
        var sanctuaryRegionDtos = new List<Transferable>();
        foreach (IPersistanceTransferStruct transferSanctuaryRegionDto in transferSanctuaryRegionDtos)
        {
            sanctuaryRegionDtos.Add(GetDtoFromTransferable(transferSanctuaryRegionDto));
        }

        return sanctuaryRegionDtos;
    }
    
    public override void RemoveEntity(string slug)
    {
        _persistenceService.RemoveEntity(slug);
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