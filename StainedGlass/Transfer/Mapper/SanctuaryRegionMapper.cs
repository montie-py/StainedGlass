using StainedGlass.Persistence.Templates;
using StainedGlass.Transfer.DTOs;

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
    
    public override Transferable? GetDTOBySlug(string slug)
    {
        var nullableTransferSanctuaryRegionDto = _persistenceService.GetDto(slug);
        if (nullableTransferSanctuaryRegionDto == null)
        {
            return null;
        }
        
        var transferSanctuaryRegionDto = (SanctuaryRegionDTO)nullableTransferSanctuaryRegionDto;

        return new SanctuaryRegionDTO
        {
            Name = transferSanctuaryRegionDto.Name,
            Slug = transferSanctuaryRegionDto.Slug,
            Description = transferSanctuaryRegionDto.Description,
            Image = transferSanctuaryRegionDto.Image,
        };
    }
    
    public override IEnumerable<Transferable?> GetAllDTOs()
    {
        var transferSanctuaryRegionDtos =
            _persistenceService.GetAllDtos() as IEnumerable<Persistence.Transfer.SanctuaryRegionDTO>;
        var sanctuaryRegionDtos = new List<Transferable>();
        foreach (var transferSanctuaryRegionDto in transferSanctuaryRegionDtos)
        {
            var sanctuaryRegionDto = new SanctuaryRegionDTO
            {
                Name = transferSanctuaryRegionDto.Name,
                Slug = transferSanctuaryRegionDto.Slug,
                Description = transferSanctuaryRegionDto.Description,
                Image = transferSanctuaryRegionDto.Image,
            };
            sanctuaryRegionDtos.Add(sanctuaryRegionDto);
        }

        return sanctuaryRegionDtos;
    }
    
    public override void RemoveEntity(string slug)
    {
        _persistenceService.RemoveEntity(slug);
    }
}