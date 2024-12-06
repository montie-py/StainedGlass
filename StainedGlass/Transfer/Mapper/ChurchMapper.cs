using StainedGlass.Persistence.Services.Entities;
using StainedGlass.Persistence.Templates;
using StainedGlass.Transfer.DTOs;

namespace StainedGlass.Transfer.Mapper;

internal class ChurchMapper : Mapper
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

    public override void ReplaceEntity(string slug, Transferable transferable)
    {
        Persistence.Transfer.ChurchDTO transferChurchDto = transferable as ChurchDTO;
        _persistenceService.ReplaceEntity(slug, transferChurchDto);
    }

    public override Transferable? GetDTOBySlug(string slug)
    {
        var nullableTransferChurchDto = _persistenceService.GetDtoBySlug(slug);
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
    
    public override void RemoveEntity(string slug)
    {
        _persistenceService.RemoveEntity(slug);
    }
}