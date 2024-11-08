using StainedGlass.Entities.Transfer;

namespace StainedGlass.Entities{
    internal class StainedGlass : Entity
    {
        public required string Slug {get; set;}
        public required string Title{get; set;}
        public required string Description{get; set;}
        public required string Image{get; set;}
        public required SanctuaryRegion SanctuaryRegion{get; set;}

        public override bool Equals(object? obj)
        {
            if (obj == null || GetType() != obj.GetType()) return false;
            StainedGlass? other = obj as StainedGlass;
            return Slug == other.Slug && Title == other.Title && Description == other.Description;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Slug, Title, Description);
        }

        public void Save()
        {
            EntitiesCollection.StainedGlasses.Add((StainedGlass)this);
        }
    }
}