namespace StainedGlass.Transfer.DTOs;

public class ChurchDTO : Transferable
{
    public required string Slug {get; set;}
    public required string Name {get; set;}
    public required string Image {get; set;}
    public required HashSet<SanctuarySideDTO> Sides {get; set;}
}