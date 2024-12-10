using StainedGlass.Persistence.Services.Entities;
using StainedGlass.Persistence.Templates;
using StainedGlass.Persistence.Transfer;
using ChurchDTO = StainedGlass.Transfer.DTOs.ChurchDTO;

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

        return GetDtoFromTransferable(nullableTransferChurchDto);
    }
    
    public override IEnumerable<Transferable> GetAllDTOs()
    {
        var transferChurchDtos = _persistenceService.GetAllDtos();
        List<ChurchDTO> churchDtos = new();
        foreach (IPersistanceTransferStruct transferChurchDto in transferChurchDtos)
        {
            churchDtos.Add((ChurchDTO)GetDtoFromTransferable(transferChurchDto));
        }

        return churchDtos;
    }
    
    public override void RemoveEntity(string slug)
    {
        _persistenceService.RemoveEntity(slug);
    }
    
    protected override Transferable GetDtoFromTransferable(IPersistanceTransferStruct transferStruct)
    {
        var churchTransferStruct = (Persistence.Transfer.ChurchDTO)transferStruct;

        return new ChurchDTO
        {
            Name = churchTransferStruct.Name,
            Slug = churchTransferStruct.Slug,
            Image = churchTransferStruct.Image,
            Description = churchTransferStruct.Description,
        };
    }
}