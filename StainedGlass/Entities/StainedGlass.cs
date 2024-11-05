namespace StainedGlass.Entities{
    internal class StainedGlass : Entity
    {
        internal required string Slug {get; set;}
        internal required string Title{get; set;}
        internal required string Description{get; set;}
        internal required string Image{get; set;}
        internal required SanctuaryRegion SanctuaryRegion{get; set;}

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
    }
}