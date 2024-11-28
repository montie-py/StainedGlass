using StainedGlass.Entities;
using StainedGlass.Entities.Transfer;
using StainedGlass.Transfer.DTOs;

namespace StainedGlass.Transfer.Mapper;

internal class ChurchMapper : NonRelatable
{

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

    public Transferable? GetDTOBySlug(string slug)
    {
        if (!EntitiesCollection.Churches.ContainsKey(slug))
        {
            return null;
        }
        return GetDTO(EntitiesCollection.Churches[slug]);
    }

    public IEnumerable<Transferable?> GetAllDTOs()
    {
        return EntitiesCollection.Churches.Select(e => GetDTO(e.Value));
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