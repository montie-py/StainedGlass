using StainedGlass.Entities;
using StainedGlass.Transfer.DTOs;

namespace StainedGlass.Transfer.Mapper;

internal class ChurchMapper : Mappable
{
    SanctuarySideMapper sanctuarySideMapper = new();

    public Entity GetEntity(Transferable transferable)
    {
        ChurchDTO churchDTO = transferable as ChurchDTO;
        HashSet<SanctuarySide> sides = new();

        foreach (var sideDTO in churchDTO.Sides)
        {
            sides.Add(sanctuarySideMapper.GetEntity(sideDTO) as SanctuarySide);
        }

        return new Church
        {
            Slug = churchDTO.Slug,
            Name = churchDTO.Name,
            Image = churchDTO.Image,
            Sides = sides
        };
    }

    public Transferable GetDTO(Entity entity)
    {
        Church church = entity as Church;
        HashSet<SanctuarySideDTO> sidesDTO = new();

        foreach (var side in church.Sides)
        {
            sidesDTO.Add(sanctuarySideMapper.GetDTO(side) as SanctuarySideDTO);
        }

        return new ChurchDTO
        {
            Slug = church.Slug,
            Name = church.Name,
            Image = church.Image,
            Sides = sidesDTO
        };
    }
}