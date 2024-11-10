using StainedGlass.Entities;
using StainedGlass.Entities.Transfer;
using StainedGlass.Transfer.DTOs;

namespace StainedGlass.Transfer.Mapper;

internal class ChurchMapper : Mappable
{

    public Transferable? GetDTO(Entity? entity)
    {
        SanctuarySideMapper sanctuarySideMapper = new();

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

    public Transferable GetDTOBySlug(string slug)
    {
        return GetDTO(EntitiesCollection.Churches.FirstOrDefault(e => e.Slug.Equals(slug)));
    }

    public Entity GetEntity(Transferable transferable)
    {
        ChurchDTO churchDTO = transferable as ChurchDTO;

        return new Church
        {
            Slug = churchDTO.Slug,
            Name = churchDTO.Name,
            Image = churchDTO.Image,
            Sides = null
        };
    }
}