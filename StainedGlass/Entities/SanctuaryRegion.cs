namespace StainedGlass.Entities{
    public class SanctuaryRegion
    {
        public required string Slug {get; set;}
        public required string Name {get; set;}
        public required string Image {get; set;}
        public required HashSet<StainedGlass> Windows {get; set;}
    }
}