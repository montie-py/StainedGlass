using StainedGlass.Persistence.Entities;
using StainedGlass.Persistence.Transfer;

namespace StainedGlass.Persistence.Services.Entities;

public class EntityChurch : INonRelatable, IPersistenceService
{
    public async Task<bool> AddEntity(IPersistanceTransferStruct transferStruct)
    {
        var churchDto = (ChurchDTO)transferStruct;
        var entity = GetEntity(churchDto) as Church;
        EntitiesCollection.Churches.TryAdd(churchDto.Slug, entity);
        return true;
    }

    public async Task<ICollection<IPersistanceTransferStruct>> GetAllDtos()
    {
        return EntitiesCollection.Churches.Select(e => GetDTOForEntity(e.Value)).ToList();
    }

    public async Task<IPersistanceTransferStruct?> GetDtoBySlug(string slug, bool includeChildrenToTheResponse)
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
    
    public async Task<bool> ReplaceEntity(string slug, IPersistanceTransferStruct transferStruct)
    {
        if (!EntitiesCollection.Churches.ContainsKey(slug))
        {
            return false;
        }

        var entity = GetEntity(transferStruct);
        entity.Slug = slug;
        var oldEntity = EntitiesCollection.Churches[slug];
        EntitiesCollection.Churches[slug] = (Church)entity;
        EntitiesCollection.Churches[slug].SanctuarySides = oldEntity.SanctuarySides;
        return true;
    }

    public async Task<bool> RemoveEntity(string slug)
    {
        EntitiesCollection.Churches.Remove(slug);
        return true;
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