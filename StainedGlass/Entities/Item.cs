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
        
        public Dictionary<string, Item> RelatedItems {get; set;}
        public required SanctuaryRegion SanctuaryRegion{get; set;}

        public void Save()
        {
            EntitiesCollection.Items.Add(Slug, this);
        }

        public void Replace(string slug, Entity entity)
        {
            entity.Slug = slug;
            EntitiesCollection.Items[slug] = (Item)entity;
        }

        public void Remove(string slug)
        {
            EntitiesCollection.Items.Remove(slug);
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