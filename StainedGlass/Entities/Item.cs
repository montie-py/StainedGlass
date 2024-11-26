using StainedGlass.Entities.Transfer;
using StainedGlass.Transfer.DTOs;
using StainedGlass.Transfer.Mapper;

namespace StainedGlass.Entities{
    internal class Item : Entity
    {
        public required string Slug {get; set;}
        public required string Title{get; set;}
        public required string Description{get; set;}
        public required string Image{get; set;}
        public required ItemType ItemType{get; set;}

        public Dictionary<string, Item> RelatedItems { get; set; } = new();
        public required SanctuaryRegion SanctuaryRegion{get; set;}

        public void Save()
        {
            EntitiesCollection.Items.TryAdd(Slug, this);
        }

        public void Replace(string slug, Entity entity)
        {
            entity.Slug = slug;
            var oldEntity = EntitiesCollection.Items[slug];
            EntitiesCollection.Items[slug] = (Item)entity;
            EntitiesCollection.Items[slug].RelatedItems = oldEntity.RelatedItems;
            EntitiesCollection.Items[slug].ItemType = oldEntity.ItemType;
            EntitiesCollection.Items[slug].SanctuaryRegion = oldEntity.SanctuaryRegion;
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
}