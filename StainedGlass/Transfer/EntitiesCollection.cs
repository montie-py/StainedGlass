namespace StainedGlass.Entities.Transfer;

internal static class EntitiesCollection
{
    internal static List<Church> Churches {get; set;} = new();
    internal static List<SanctuarySide> SanctuarySides {get; set;} = new();
    internal static List<SanctuaryRegion> SanctuaryRegions {get; set;} = new();
    internal static List<StainedGlass> StainedGlasses {get; set;} = new();
}