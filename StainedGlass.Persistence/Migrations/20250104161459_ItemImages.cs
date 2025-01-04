using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StainedGlass.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class ItemImages : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ItemImage",
                columns: table => new
                {
                    Slug = table.Column<string>(type: "TEXT", nullable: false),
                    ItemSlug = table.Column<string>(type: "TEXT", nullable: false),
                    Image = table.Column<byte[]>(type: "BLOB", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemImage", x => x.Slug);
                    table.ForeignKey(
                        name: "FK_ItemImage_Items_ItemSlug",
                        column: x => x.ItemSlug,
                        principalTable: "Items",
                        principalColumn: "Slug",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ItemImage_ItemSlug",
                table: "ItemImage",
                column: "ItemSlug");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ItemImage");
        }
    }
}
