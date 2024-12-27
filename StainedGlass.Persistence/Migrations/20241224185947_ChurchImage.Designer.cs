﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using StainedGlass.Persistence;

#nullable disable

namespace StainedGlass.Persistence.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20241224185947_ChurchImage")]
    partial class ChurchImage
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "9.0.0");

            modelBuilder.Entity("StainedGlass.Persistence.Entities.Church", b =>
                {
                    b.Property<string>("Slug")
                        .HasColumnType("TEXT");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<byte[]>("Image")
                        .IsRequired()
                        .HasColumnType("BLOB");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Slug");

                    b.ToTable("Churches");
                });

            modelBuilder.Entity("StainedGlass.Persistence.Entities.Item", b =>
                {
                    b.Property<string>("Slug")
                        .HasColumnType("TEXT");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Image")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("ItemTypeSlug")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("SanctuaryRegionSlug")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Slug");

                    b.HasIndex("ItemTypeSlug");

                    b.HasIndex("SanctuaryRegionSlug");

                    b.ToTable("Items");
                });

            modelBuilder.Entity("StainedGlass.Persistence.Entities.ItemRelation", b =>
                {
                    b.Property<string>("ItemSlug")
                        .HasColumnType("TEXT");

                    b.Property<string>("RelatedItemSlug")
                        .HasColumnType("TEXT");

                    b.HasKey("ItemSlug", "RelatedItemSlug");

                    b.HasIndex("RelatedItemSlug");

                    b.ToTable("ItemRelations");
                });

            modelBuilder.Entity("StainedGlass.Persistence.Entities.ItemType", b =>
                {
                    b.Property<string>("Slug")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Slug");

                    b.ToTable("ItemTypes");
                });

            modelBuilder.Entity("StainedGlass.Persistence.Entities.SanctuaryRegion", b =>
                {
                    b.Property<string>("Slug")
                        .HasColumnType("TEXT");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Image")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("SanctuarySideSlug")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Slug");

                    b.HasIndex("SanctuarySideSlug");

                    b.ToTable("SanctuaryRegions");
                });

            modelBuilder.Entity("StainedGlass.Persistence.Entities.SanctuarySide", b =>
                {
                    b.Property<string>("Slug")
                        .HasColumnType("TEXT");

                    b.Property<string>("ChurchSlug")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Slug");

                    b.HasIndex("ChurchSlug");

                    b.ToTable("SanctuarySides");
                });

            modelBuilder.Entity("StainedGlass.Persistence.Entities.Item", b =>
                {
                    b.HasOne("StainedGlass.Persistence.Entities.ItemType", "ItemType")
                        .WithMany("Items")
                        .HasForeignKey("ItemTypeSlug")
                        .OnDelete(DeleteBehavior.SetNull)
                        .IsRequired();

                    b.HasOne("StainedGlass.Persistence.Entities.SanctuaryRegion", "SanctuaryRegion")
                        .WithMany("Items")
                        .HasForeignKey("SanctuaryRegionSlug")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ItemType");

                    b.Navigation("SanctuaryRegion");
                });

            modelBuilder.Entity("StainedGlass.Persistence.Entities.ItemRelation", b =>
                {
                    b.HasOne("StainedGlass.Persistence.Entities.Item", "Item")
                        .WithMany("RelatedItems")
                        .HasForeignKey("ItemSlug")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("StainedGlass.Persistence.Entities.Item", "RelatedItem")
                        .WithMany()
                        .HasForeignKey("RelatedItemSlug")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Item");

                    b.Navigation("RelatedItem");
                });

            modelBuilder.Entity("StainedGlass.Persistence.Entities.SanctuaryRegion", b =>
                {
                    b.HasOne("StainedGlass.Persistence.Entities.SanctuarySide", "SanctuarySide")
                        .WithMany("Regions")
                        .HasForeignKey("SanctuarySideSlug")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("SanctuarySide");
                });

            modelBuilder.Entity("StainedGlass.Persistence.Entities.SanctuarySide", b =>
                {
                    b.HasOne("StainedGlass.Persistence.Entities.Church", "Church")
                        .WithMany("SanctuarySides")
                        .HasForeignKey("ChurchSlug")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Church");
                });

            modelBuilder.Entity("StainedGlass.Persistence.Entities.Church", b =>
                {
                    b.Navigation("SanctuarySides");
                });

            modelBuilder.Entity("StainedGlass.Persistence.Entities.Item", b =>
                {
                    b.Navigation("RelatedItems");
                });

            modelBuilder.Entity("StainedGlass.Persistence.Entities.ItemType", b =>
                {
                    b.Navigation("Items");
                });

            modelBuilder.Entity("StainedGlass.Persistence.Entities.SanctuaryRegion", b =>
                {
                    b.Navigation("Items");
                });

            modelBuilder.Entity("StainedGlass.Persistence.Entities.SanctuarySide", b =>
                {
                    b.Navigation("Regions");
                });
#pragma warning restore 612, 618
        }
    }
}