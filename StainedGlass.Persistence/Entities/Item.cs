

namespace StainedGlass.Persistence.Entities;
internal class Item : IEntity
{
    public string Slug {get; set;}
    public string Title{get; set;}
    public string Description{get; set;}
    public string Image{get; set;}
    public string ItemTypeSlug {get; set;}
    public ItemType ItemType{get; set;}
    public string SanctuaryRegionSlug {get; set;}
    public SanctuaryRegion SanctuaryRegion{get; set;}
    public ICollection<ItemRelation> RelatedItems {get; set;}
    public ICollection<ItemRelation> RelatedToItems {get; set;}
    

    public void Save()
    {
        EntitiesCollection.Items.TryAdd(Slug, this);
    }

    public void Replace(string slug, IEntity entity)
    {
        if (!EntitiesCollection.Items.ContainsKey(slug))
        {
            return;
        }
        entity.Slug = slug;
        var oldEntity = EntitiesCollection.Items[slug];
        EntitiesCollection.Items[slug] = (Item)entity;
        //if old entity has an assigned itemType - new one cannot lack one
        if (EntitiesCollection.Items[slug].ItemType is null)
        {
            EntitiesCollection.Items[slug].ItemType = oldEntity.ItemType;
        }
        //if old entity has an assigned region - new one cannot lack one
        if (EntitiesCollection.Items[slug].SanctuaryRegion is null)
        {
            EntitiesCollection.Items[slug].SanctuaryRegion = oldEntity.SanctuaryRegion;
        }

        //if the item has related items - update this related item with the newer version
        if (RelatedItems.Count > 0)
        {
            foreach (var relatedItem in RelatedItems.Values)
            {
                //if a related item doesn't have the present item as a related one - add it
                if (!relatedItem.RelatedItems.ContainsKey(Slug))
                {
                    relatedItem.RelatedItems.Add(Slug, (Item)entity);
                }
                else
                {
                    relatedItem.RelatedItems[Slug] = (Item)entity;   
                }
            }
        }
    }

    public void Remove()
    {
        //remove item from its region
        var sanctuaryRegion = EntitiesCollection.SanctuaryRegions.Values.FirstOrDefault(e => 
            e.Items?.FirstOrDefault(i => i.Slug == Slug) != null
            );
        if (sanctuaryRegion != null)
        {
            sanctuaryRegion.Items?.RemoveWhere(e => e.Slug == Slug);
        }

        //remove this item from its related items as well
        if (RelatedItems.Count > 0)
        {
            foreach (var relatedItem in RelatedItems.Values)
            {
                relatedItem.RelatedItems.Remove(Slug);
            }
        }
        EntitiesCollection.Items.Remove(Slug);
    }

    public override bool Equals(object? obj)
    {
        if (obj == null || GetType() != obj.GetType()) return false;
        Item? other = obj as Item;
        return Slug == other.Slug && Title == other.Title && Description == other.Description;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Slug, Title, Description);
    }
}