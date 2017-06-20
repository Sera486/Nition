using Microsoft.EntityFrameworkCore.Migrations;

namespace Nition.Data.Migrations
{
    public partial class Updatedthemeentity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImageURL",
                table: "Themes",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageURL",
                table: "Themes");
        }
    }
}
