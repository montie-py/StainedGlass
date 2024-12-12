using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StainedGlass.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Churches",
                columns: table => new
                {
                    Slug = table.Column<string>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false),
                    Image = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Churches", x => x.Slug);
                });

            migrationBuilder.CreateTable(
                name: "ItemTypes",
                columns: table => new
                {
                    Slug = table.Column<string>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemTypes", x => x.Slug);
                });

            migrationBuilder.CreateTable(
                name: "SanctuarySides",
                columns: table => new
                {
                    Slug = table.Column<string>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    ChurchSlug = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SanctuarySides", x => x.Slug);
                    table.ForeignKey(
                        name: "FK_SanctuarySides_Churches_ChurchSlug",
                        column: x => x.ChurchSlug,
                        principalTable: "Churches",
                        principalColumn: "Slug",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SanctuaryRegions",
                columns: table => new
                {
                    Slug = table.Column<string>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false),
                    Image = table.Column<string>(type: "TEXT", nullable: false),
                    SanctuarySideSlug = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SanctuaryRegions", x => x.Slug);
                    table.ForeignKey(
                        name: "FK_SanctuaryRegions_SanctuarySides_SanctuarySideSlug",
                        column: x => x.SanctuarySideSlug,
                        principalTable: "SanctuarySides",
                        principalColumn: "Slug",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Items",
                columns: table => new
                {
                    Slug = table.Column<string>(type: "TEXT", nullable: false),
                    Title = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false),
                    Image = table.Column<string>(type: "TEXT", nullable: false),
                    ItemTypeSlug = table.Column<string>(type: "TEXT", nullable: false),
                    SanctuaryRegionSlug = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Items", x => x.Slug);
                    table.ForeignKey(
                        name: "FK_Items_ItemTypes_ItemTypeSlug",
                        column: x => x.ItemTypeSlug,
                        principalTable: "ItemTypes",
                        principalColumn: "Slug",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Items_SanctuaryRegions_SanctuaryRegionSlug",
                        column: x => x.SanctuaryRegionSlug,
                        principalTable: "SanctuaryRegions",
                        principalColumn: "Slug",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ItemRelations",
                columns: table => new
                {
                    ItemSlug = table.Column<string>(type: "TEXT", nullable: false),
                    RelatedItemSlug = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemRelations", x => new { x.ItemSlug, x.RelatedItemSlug });
                    table.ForeignKey(
                        name: "FK_ItemRelations_Items_ItemSlug",
                        column: x => x.ItemSlug,
                        principalTable: "Items",
                        principalColumn: "Slug",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_ItemRelations_Items_RelatedItemSlug",
                        column: x => x.RelatedItemSlug,
                        principalTable: "Items",
                        principalColumn: "Slug",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ItemRelations_RelatedItemSlug",
                table: "ItemRelations",
                column: "RelatedItemSlug");

            migrationBuilder.CreateIndex(
                name: "IX_Items_ItemTypeSlug",
                table: "Items",
                column: "ItemTypeSlug");

            migrationBuilder.CreateIndex(
                name: "IX_Items_SanctuaryRegionSlug",
                table: "Items",
                column: "SanctuaryRegionSlug");

            migrationBuilder.CreateIndex(
                name: "IX_SanctuaryRegions_SanctuarySideSlug",
                table: "SanctuaryRegions",
                column: "SanctuarySideSlug");

            migrationBuilder.CreateIndex(
                name: "IX_SanctuarySides_ChurchSlug",
                table: "SanctuarySides",
                column: "ChurchSlug");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ItemRelations");

            migrationBuilder.DropTable(
                name: "Items");

            migrationBuilder.DropTable(
                name: "ItemTypes");

            migrationBuilder.DropTable(
                name: "SanctuaryRegions");

            migrationBuilder.DropTable(
                name: "SanctuarySides");

            migrationBuilder.DropTable(
                name: "Churches");
        }
    }
}
