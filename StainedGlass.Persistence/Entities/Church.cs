using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace StainedGlass.Persistence.Entities;

internal class Church : IEntity
{
    [Key]
    public required string Slug {get; set;}
    public string Name {get; set;}
    public string Description {get; set;}
    public byte[] Image {get; set;}
    public ICollection<SanctuarySide>? SanctuarySides { get; set; }
}