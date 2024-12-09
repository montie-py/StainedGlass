using System.ComponentModel.DataAnnotations;
using StainedGlass.Persistence.Services.Entities;

namespace StainedGlass.Persistence.Entities;

internal class Church : IEntity
{
    [Key]
    public required string Slug {get; set;}
    public string Name {get; set;}
    public string Description {get; set;}
    public string Image {get; set;}
    public ICollection<SanctuarySide>? SanctuarySides { get; set; }
}