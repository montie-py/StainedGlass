using Microsoft.EntityFrameworkCore;
using StainedGlass.Persistence.Entities;

namespace StainedGlass.Persistence;

internal class AppDbContext : DbContext
{
    internal DbSet<Church> Churches { get; set; }
    internal DbSet<SanctuarySide> SanctuarySides { get; set; }
    internal DbSet<SanctuaryRegion> SanctuaryRegions { get; set; }
    internal DbSet<Item> Items { get; set; }
    internal DbSet<ItemType> ItemTypes { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source=StainedGlass.db");
    }
}