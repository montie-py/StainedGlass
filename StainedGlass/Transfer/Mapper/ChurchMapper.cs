using StainedGlass.Persistence.Services.Entities;
using StainedGlass.Persistence.Templates;
using StainedGlass.Transfer.DTOs;

namespace StainedGlass.Transfer.Mapper;

internal class ChurchMapper : Mapper, NonRelatable
{
    public override void SetInstance(IPersistenceTemplate template)
    {
        _persistenceService = template.GetChurchInstance();
    } 
    
    public override void SaveEntity(Transferable transferable)
    {
        Persistence.Transfer.ChurchDTO transferChurchDto = transferable as ChurchDTO;
        _persistenceService.AddEntity(transferChurchDto);
    }
    
    public override Transferable? GetDTOBySlug(string slug)
    {
        var nullableTransferChurchDto = _persistenceService.GetDto(slug);
        if (nullableTransferChurchDto is null)
        {
            return null;
        }

        var transferChurchDto = (Persistence.Transfer.ChurchDTO)nullableTransferChurchDto;

        return new ChurchDTO
        {
            Name = transferChurchDto.Name,
            Slug = transferChurchDto.Slug,
            Image = transferChurchDto.Image,
            Description = transferChurchDto.Description,
        };
    }
    
    public override IEnumerable<Transferable> GetAllDTOs()
    {
        var transferChurchDtos = _persistenceService.GetAllDtos() as IEnumerable<Persistence.Transfer.ChurchDTO>;
        List<ChurchDTO> churchDtos = new();
        foreach (var transferChurchDto in transferChurchDtos)
        {
            ChurchDTO dto = new ChurchDTO
            {
                Name = transferChurchDto.Name,
                Slug = transferChurchDto.Slug,
                Image = transferChurchDto.Image,
                Description = transferChurchDto.Description
            };
            churchDtos.Add(dto);
        }

        return churchDtos;
    }
    
    public Transferable? GetDTO(Entity? entity, bool skipParentElements = false, bool skipChildrenElements = false)
    {
        if (entity == null) 
        {
            return null;
        }
        
        SanctuarySideMapper sanctuarySideMapper = new();

        Church church = entity as Church;
        HashSet<SanctuarySideDTO> sidesDTO = new();

        if (!skipChildrenElements)
        {
            foreach (var side in church.Sides)
            {
                sidesDTO.Add(sanctuarySideMapper.GetDTO(side, skipParentElements: true) as SanctuarySideDTO);
            }
        }

        return new ChurchDTO
        {
            Slug = church.Slug,
            Name = church.Name,
            Description = church.Description,
            Image = church.Image,
            Sides = sidesDTO
        };
    }

    public Entity GetEntity(Transferable transferable)
    {
        ChurchDTO churchDTO = transferable as ChurchDTO;

        Church churchEntity = new Church
        {
            Slug = churchDTO.Slug,
            Name = churchDTO.Name,
            Description = churchDTO.Description,
            Image = churchDTO.Image,
        };

        if (churchDTO.Sides != null)
        {
            SanctuarySideMapper sanctuarySideMapper = new();

            churchEntity.Sides = churchDTO.Sides.Select(
                    e => sanctuarySideMapper.GetEntity(e) as SanctuarySide
                    )
                .ToHashSet();
        }

        return churchEntity;
    }

    public void RemoveEntity(string slug)
    {
        if (EntitiesCollection.Churches.ContainsKey(slug))
        {
            EntitiesCollection.Churches[slug].Remove();
        }
    }
}