using Microsoft.AspNetCore.Http;
using StainedGlass.Persistence.Entities;

namespace StainedGlass.Persistence.Transfer
{
    public interface 
        IPersistanceTransferStruct
    {
        
    }
    public struct ChurchDTO : IPersistanceTransferStruct
    {
        public string Slug {get; set;}
        public string Name {get; set;}
        public string Description {get; set;}
        public IFormFile Image {get; set;}
        public  HashSet<SanctuarySideDTO>? Sides { get; set; }
    }

    public struct SanctuarySideDTO : IPersistanceTransferStruct
    {
        public string Slug { get; set; }
        public string Name { get; set; }
        public string Position { get; set; }
        public List<SanctuaryRegionDTO> Regions { get; set; }
        public string ChurchSlug { get; set; }
        public ChurchDTO? Church { get; set; }
    }

    public struct SanctuaryRegionDTO : IPersistanceTransferStruct
    {
        public string Slug {get; set;}
        public string Name {get; set;}
        public string Description {get; set;}
        public IFormFile Image {get; set;}
        public HashSet<ItemDTO>? Items { get; set; }
        public string SanctuarySideSlug {get; set;}
        public SanctuarySideDTO? SanctuarySide {get; set;}
    }

    public struct ItemDTO : IPersistanceTransferStruct
    {
        public string Slug {get; set;}
        public string Title{get; set;}
        public string Description{get; set;}
        public string Position{get; set;}
        public string ItemTypeSlug {get; set;}
        public List<IFormFile> ItemImages { get; set; }
        public ItemTypeDTO ItemType{get; set;}

        public string? SanctuaryRegionSlug { get; set; }
        public SanctuaryRegionDTO? SanctuaryRegion { set; get; }
        public Dictionary<string, ItemDTO>? RelatedItems { get; set; }
        public HashSet<string>? RelatedItemsSlugs { get; set; }

    }
    
    public struct ItemTypeDTO : IPersistanceTransferStruct
    {
        public string Name { get; set; }
        public string Slug { get; set; }
        public string IconSlug { get; set; }
        public ICollection<ItemDTO> Items { get; set; }
    }

    public struct ItemImageDTO
    {
        public string Slug { get; set; }
        public string ItemSlug { get; set; }
        public IFormFile Image { get; set; }
    }
}