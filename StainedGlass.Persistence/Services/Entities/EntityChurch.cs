using StainedGlass.Persistence.Entities;
using StainedGlass.Persistence.Transfer;
using StainedGlass.Transfer.Mapper;

namespace StainedGlass.Persistence.Services.Entities;

public class EntityChurch : INonRelatable, IPersistenceService
{
    public void AddEntity(IPersistanceTransferStruct transferStruct)
    {
        var churchDto = (ChurchDTO)transferStruct;
        var entity = new Church
        {
            Name = churchDto.Name,
            Slug = churchDto.Slug,
            Description = churchDto.Description,
            Image = churchDto.Image,
        };
        EntitiesCollection.Churches.TryAdd(churchDto.Slug, entity);    
    }

    public IEnumerable<IPersistanceTransferStruct> GetAllDtos()
    {
        var result = new List<IPersistanceTransferStruct>();

        foreach (string churchSlug in EntitiesCollection.Churches.Keys)
        {
            result.Add(GetDtoBySlug(churchSlug));
        }
        return result;
    }

    private IPersistanceTransferStruct? GetDtoBySlug(string slug)
    {
        if (!EntitiesCollection.Churches.ContainsKey(slug))
        {
            return null;
        }
        return GetDTO(EntitiesCollection.Churches[slug]);
    }

    public IPersistanceTransferStruct? GetDto(string slug)
    {
        
    }
    
    public void ReplaceEntity(string slug, IPersistanceTransferStruct transferStruct)
    {
        if (!EntitiesCollection.Churches.ContainsKey(slug))
        {
            return;
        }
        entity.Slug = slug;
        var oldEntity = EntitiesCollection.Churches[slug];
        EntitiesCollection.Churches[slug] = (Church)entity;
        EntitiesCollection.Churches[slug].Sides = oldEntity.Sides;
    }

    public void RemoveEntity(string slug)
    {
        EntitiesCollection.Churches.Remove(slug);
    }

    public IPersistanceTransferStruct? GetDTO(Entity? entity, bool skipParentElements = false, bool skipChildrenElements = false)
    {
        if (entity == null) 
        {
            return null;
        }
        
        SanctuarySideMapper sanctuarySideMapper = new();

        EntityChurch church = entity as EntityChurch;
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

        EntityChurch churchEntity = new EntityChurch
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
}