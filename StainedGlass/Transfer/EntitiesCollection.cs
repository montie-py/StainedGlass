namespace StainedGlass.Entities.Transfer;

internal static class EntitiesCollection
{
    internal static List<Church> Churches {get; set;} = new();
    internal static List<SanctuarySide> SanctuarySides {get; set;} = new();
    internal static List<SanctuaryRegion> SanctuaryRegions {get; set;} = new();
    internal static List<Item> Items {get; set;} = new();
    internal static Dictionary<string, ItemType> ItemsTypes {get; set;} = new();
}