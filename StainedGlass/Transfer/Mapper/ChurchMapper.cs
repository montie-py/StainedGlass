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
    
    public override async Task<bool> SaveEntity(Transferable transferable)
    {
        Persistence.Transfer.ChurchDTO transferChurchDto = transferable as ChurchDTO;
        await _persistenceService.AddEntity(transferChurchDto);
        return true;
    }

    public override async Task<bool> ReplaceEntity(string slug, Transferable transferable)
    {
        Persistence.Transfer.ChurchDTO transferChurchDto = transferable as ChurchDTO;
        return await _persistenceService.ReplaceEntity(slug, transferChurchDto);
    }

    public override async Task<Transferable?> GetDTOBySlug(string slug)
    {
        var nullableTransferChurchDto = await _persistenceService.GetDtoBySlug(slug);
        if (nullableTransferChurchDto is null)
        {
            return null;
        }

        return GetDtoFromTransferable(nullableTransferChurchDto);
    }
    
    public override async Task<ICollection<Transferable>> GetAllDTOs()
    {
        var transferChurchDtos = await _persistenceService.GetAllDtos();
        List<Transferable> churchDtos = new();
        foreach (IPersistanceTransferStruct transferChurchDto in transferChurchDtos)
        {
            churchDtos.Add(GetDtoFromTransferable(transferChurchDto));
        }

        return churchDtos;
    }
    
    public override async Task<bool> RemoveEntity(string slug)
    {
        return await _persistenceService.RemoveEntity(slug);
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