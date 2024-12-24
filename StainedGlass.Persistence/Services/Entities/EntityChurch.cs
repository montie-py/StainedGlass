using StainedGlass.Persistence.Entities;
using StainedGlass.Persistence.Transfer;
using StainedGlass.Transfer.Mapper;

namespace StainedGlass.Persistence.Services.Entities;

public class EntityChurch : INonRelatable, IPersistenceService
{
    public void AddEntity(IPersistanceTransferStruct transferStruct)
    {
        var churchDto = (ChurchDTO)transferStruct;
        var entity = GetEntity(churchDto) as Church;
        EntitiesCollection.Churches.TryAdd(churchDto.Slug, entity);    
    }

    public IEnumerable<IPersistanceTransferStruct> GetAllDtos()
    {
        return EntitiesCollection.Churches.Select(e => GetDTOForEntity(e.Value)).ToList();
    }

    public IPersistanceTransferStruct? GetDtoBySlug(string slug)
    {
        if (!EntitiesCollection.Churches.ContainsKey(slug))
        {
            return null;
        }
        return GetDTOForEntity(EntitiesCollection.Churches[slug]);
    }
    
    public IPersistanceTransferStruct? GetDTOForEntity(
        IEntity? entity, 
        bool skipParentElements = false, 
        bool skipChildrenElements = false
        )
    {
        if (entity == null) 
        {
            return null;
        }

        Church church = entity as Church;
        HashSet<SanctuarySideDTO> sidesDTO = new();

        EntitySanctuarySide entitySanctuarySide = new();

        if (!skipChildrenElements)
        {
            foreach (var side in church.SanctuarySides)
            {
                sidesDTO.Add((SanctuarySideDTO)entitySanctuarySide.GetDTOForEntity(side, skipParentElements: true));
            }
        }

        return new ChurchDTO
        {
            Slug = church.Slug,
            Name = church.Name,
            Description = church.Description,
            // Image = church.Image,
            Sides = sidesDTO
        };
    }
    
    public void ReplaceEntity(string slug, IPersistanceTransferStruct transferStruct)
    {
        if (!EntitiesCollection.Churches.ContainsKey(slug))
        {
            return;
        }

        var entity = GetEntity(transferStruct);
        entity.Slug = slug;
        var oldEntity = EntitiesCollection.Churches[slug];
        EntitiesCollection.Churches[slug] = (Church)entity;
        EntitiesCollection.Churches[slug].SanctuarySides = oldEntity.SanctuarySides;
    }

    public void RemoveEntity(string slug)
    {
        EntitiesCollection.Churches.Remove(slug);
    }

    public IEntity GetEntity(IPersistanceTransferStruct transferable)
    {
        // ChurchDTO churchDTO = (ChurchDTO)transferable;
        ChurchDTO? churchDTO = transferable as ChurchDTO?;
        if (churchDTO is null)
        {
            return null;
        }
        
        Church churchEntity = new Church
        {
            Slug = churchDTO.Value.Slug,
            Name = churchDTO.Value.Name,
            Description = churchDTO.Value.Description,
            // Image = churchDTO.Value.Image,
        };

        if (churchDTO.Value.Sides != null)
        {
            EntitySanctuarySide entitySanctuarySide = new();
            churchEntity.SanctuarySides = churchDTO.Value.Sides.Select(
                    e => entitySanctuarySide.GetEntity(e) as SanctuarySide
                )
                .ToHashSet();
        }

        return churchEntity;
    }
}