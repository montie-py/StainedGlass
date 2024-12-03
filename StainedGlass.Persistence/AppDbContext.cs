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
    internal DbSet<ItemRelation> ItemRelations { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source=StainedGlass.db");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // church-sanctuaryside relationship
        modelBuilder.Entity<Church>()
            .HasMany(church => church.SanctuarySides)
            .WithOne(sanctuarySide => sanctuarySide.Church)
            .HasForeignKey(sanctuarySide => sanctuarySide.ChurchSlug);
        
        //sanctuaryside-sanctuaryregion relationship
        modelBuilder.Entity<SanctuarySide>()
            .HasMany(sanctuarySide => sanctuarySide.Regions)
            .WithOne(region => region.SanctuarySide)
            .HasForeignKey(sanctuaryRegion => sanctuaryRegion.SanctuarySideSlug);
        
        //sanctuaryregion-item relationship
        modelBuilder.Entity<SanctuaryRegion>()
            .HasMany(sanctuaryRegion => sanctuaryRegion.Items)
            .WithOne(item => item.SanctuaryRegion)
            .HasForeignKey(item => item.SanctuaryRegionSlug);
        
        //itemtype-item relationship
        modelBuilder.Entity<ItemType>()
            .HasMany(itemType => itemType.Items)
            .WithOne(item => item.ItemType)
            .HasForeignKey(item => item.ItemTypeSlug)
            .OnDelete(DeleteBehavior.SetNull);

        //item-relateditem relationship
        modelBuilder.Entity<ItemRelation>()
            .HasKey(itemRelation => new { itemRelation.ItemSlug, itemRelation.RelatedItemSlug });
        
        modelBuilder.Entity<ItemRelation>()
            .HasOne(itemRelation => itemRelation.RelatedItem)
            .WithMany(item => item.RelatedItems)
            .HasForeignKey(itemRelation => itemRelation.ItemSlug)
            .OnDelete(DeleteBehavior.SetNull);
        
        modelBuilder.Entity<ItemRelation>()
            .HasOne(itemRelation => itemRelation.RelatedItem)
            .WithMany(item => item.RelatedToItems)
            .HasForeignKey(itemRelation => itemRelation.RelatedItemSlug)
            .OnDelete(DeleteBehavior.SetNull);
        
        // base.OnModelCreating(modelBuilder);
    }
}