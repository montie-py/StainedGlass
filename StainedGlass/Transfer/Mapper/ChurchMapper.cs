using StainedGlass.Persistence.Templates;
using StainedGlass.Persistence.Transfer;
using ChurchDTO = StainedGlass.Transfer.DTOs.ChurchDTO;
using SanctuarySideDTO = StainedGlass.Transfer.DTOs.SanctuarySideDTO;

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

    public override async Task<Transferable?> GetDTOBySlug(string slug, bool includeChildrenToTheResponse)
    {
        var nullableTransferChurchDto = await _persistenceService.GetDtoBySlug(slug, includeChildrenToTheResponse);
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
        
        var sanctuarySideDTOs = new HashSet<SanctuarySideDTO>();

        if (churchTransferStruct.Sides.Count > 0)
        {
            foreach (var sanctuarySide in churchTransferStruct.Sides)
            {
                sanctuarySideDTOs.Add(new SanctuarySideDTO
                {
                    Name = sanctuarySide.Name,
                    Slug = sanctuarySide.Slug,
                    Position = sanctuarySide.Position
                });
            }
        }

        return new ChurchDTO
        {
            Name = churchTransferStruct.Name,
            Slug = churchTransferStruct.Slug,
            Image = churchTransferStruct.Image,
            Description = churchTransferStruct.Description,
            Sides = sanctuarySideDTOs
        };
    }
}