namespace StainedGlass.Entities{
    public class SanctuaryRegion
    {
        public string Slug {get; set;}
        public string Name {get; set;}
        public string Image {get; set;}
        public HashSet<StainedGlass> Windows {get; set;}
    }
}