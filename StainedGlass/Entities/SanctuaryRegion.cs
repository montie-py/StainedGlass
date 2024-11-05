namespace StainedGlass.Entities{
    internal class SanctuaryRegion : Entity
    {
        internal required string Slug {get; set;}
        internal required string Name {get; set;}
        internal required string Image {get; set;}
        internal required HashSet<StainedGlass> Windows {get; set;}
        internal required SanctuarySide SanctuarySide {get; set;}

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