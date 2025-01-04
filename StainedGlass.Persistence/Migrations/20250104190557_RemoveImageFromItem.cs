using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StainedGlass.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class RemoveImageFromItem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Image",
                table: "Items");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Image",
                table: "Items",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }
    }
}
