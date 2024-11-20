namespace StainedGlass.Entities.Transfer;

internal static class EntitiesCollection
{
    internal static Dictionary<string, Church> Churches {get; set;} = new();
    internal static Dictionary<string, SanctuarySide> SanctuarySides {get; set;} = new();
    internal static Dictionary<string, SanctuaryRegion> SanctuaryRegions {get; set;} = new();
    internal static Dictionary<string, Item> Items {get; set;} = new();
    internal static Dictionary<string, ItemType> ItemsTypes {get; set;} = new();
}