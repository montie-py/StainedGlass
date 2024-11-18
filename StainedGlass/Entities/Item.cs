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
        
        public HashSet<Item> RelatedItems {get; set;}
        public required SanctuaryRegion SanctuaryRegion{get; set;}

        public void Save()
        {
            EntitiesCollection.Items.Add((Item)this);
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