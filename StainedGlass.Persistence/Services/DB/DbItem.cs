using StainedGlass.Persistence.Entities;
using StainedGlass.Persistence.Transfer;

namespace StainedGlass.Persistence.Services.DB;

internal class DbItem : DatabasePersistenceService
{
    public override void AddEntity(IPersistanceTransferStruct transferStruct)
    {
        var itemStruct = (ItemDTO)transferStruct;
        var newItem = new Item
        {
            Title = itemStruct.Title,
            Slug = itemStruct.Slug,
            Description = itemStruct.Description,
            Image = itemStruct.Image,
            SanctuaryRegionSlug = itemStruct.SanctuaryRegionSlug,
        };
        
        _dbContext.Items.Add(newItem);
        _dbContext.SaveChanges();

        if (itemStruct.RelatedItemsSlugs != null)
        {
            foreach (var relatedItemsSlug in itemStruct.RelatedItemsSlugs)
            {
                var relation = new ItemRelation
                {
                    ItemSlug = newItem.Slug,
                    RelatedItemSlug = relatedItemsSlug,
                };
                
                _dbContext.ItemRelations.Add(relation);
                _dbContext.SaveChanges();
            }
        }
    }

    public override List<IEntity> GetEntities()
    {
        throw new NotImplementedException();
    }
}