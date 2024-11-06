namespace StainedGlass.Entities{
    public class SanctuaryRegion : Entity
    {
        public required string Slug {get; set;}
        public required string Name {get; set;}
        public required string Image {get; set;}
        public required HashSet<StainedGlass> Windows {get; set;}
        public required SanctuarySide SanctuarySide {get; set;}

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType()) return false;

            SanctuaryRegion? other = obj as SanctuaryRegion;

            return Slug == other.Slug && Name == other.Name;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Slug, Name); 
        }
    }
}