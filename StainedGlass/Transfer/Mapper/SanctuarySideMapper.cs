using StainedGlass.Persistence.Templates;
using StainedGlass.Transfer.DTOs;

namespace StainedGlass.Transfer.Mapper;

internal class SanctuarySideMapper : Mapper, NonRelatable
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
    
    public override Transferable? GetDTOBySlug(string slug)
    {
        var nullableTransferSanctuarySideDto = _persistenceService.GetDto(slug);
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
    
    public Transferable? GetDTO(Entity? entity, bool skipParentElements = false, bool skipChildrenElements = false)
    {
        if (entity == null) 
        {
            return null;
        }
        
        ChurchMapper churchMapper = new();
        SanctuaryRegionMapper sanctuaryRegionMapper = new();
        SanctuarySide? sanctuarySide = entity as SanctuarySide;

        List<SanctuaryRegionDTO> regionsDTO = new();
        ChurchDTO churchDTO = null;
        
        if (!skipChildrenElements)
        {
            foreach (SanctuaryRegion? region in sanctuarySide.Regions)
            {
                regionsDTO.Add(sanctuaryRegionMapper.GetDTO(
                    region, skipParentElements: true, skipChildrenElements: true
                ) as SanctuaryRegionDTO);
            }
        }

        var sanctuarySideDTO = new SanctuarySideDTO
        {
            Slug = sanctuarySide.Slug,
            Name = sanctuarySide.Name,
            Regions = regionsDTO,
            Church = churchDTO
        };

        if (!skipParentElements)
        {
            churchDTO = churchMapper.GetDTO(sanctuarySide.Church) as ChurchDTO;
            if (churchDTO != null)
            {
                sanctuarySideDTO.ChurchSlug = churchDTO.Slug;
            }
        }
        
        return sanctuarySideDTO;
    }

    public Entity GetEntity(Transferable transferable)
    {
        SanctuarySideDTO sanctuarySideDTO = transferable as SanctuarySideDTO;

        Church sanctuarySideChurch = 
            (
                sanctuarySideDTO.ChurchSlug != null 
                && EntitiesCollection.Churches.ContainsKey(sanctuarySideDTO.ChurchSlug)
                ) ? EntitiesCollection.Churches[sanctuarySideDTO.ChurchSlug] : null;

        SanctuarySide sanctuarySide = new SanctuarySide
        {
            Name = sanctuarySideDTO.Name,
            Slug = sanctuarySideDTO.Slug,
            Regions = null,
            Church = sanctuarySideChurch,
        };

        if (sanctuarySideDTO.Regions != null)
        {
            SanctuaryRegionMapper sanctuaryRegionMapper = new();
            sanctuarySide.Regions =
                sanctuarySideDTO.Regions.Select(
                    e => sanctuaryRegionMapper.GetEntity(e) as SanctuaryRegion
                    ).ToList();
        }

        if (sanctuarySideChurch != null) {
            sanctuarySideChurch.Sides.Add(sanctuarySide);
        }

        return sanctuarySide;
    }

    public void RemoveEntity(string slug)
    {
        if (EntitiesCollection.SanctuarySides.ContainsKey(slug))
        {
            EntitiesCollection.SanctuarySides[slug].Remove();   
        }
    }
}