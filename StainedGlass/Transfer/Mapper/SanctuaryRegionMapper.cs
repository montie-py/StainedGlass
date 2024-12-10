using StainedGlass.Persistence.Templates;
using StainedGlass.Persistence.Transfer;
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
    
    public override IEnumerable<Transferable?> GetAllDTOs()
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
        var transferSanctuaryRegionDto = (SanctuaryRegionDTO)transferStruct;

        return new SanctuaryRegionDTO
        {
            Name = transferSanctuaryRegionDto.Name,
            Slug = transferSanctuaryRegionDto.Slug,
            Description = transferSanctuaryRegionDto.Description,
            Image = transferSanctuaryRegionDto.Image,
        };
    }
}