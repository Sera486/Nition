using Microsoft.EntityFrameworkCore.Migrations;

namespace Nition.Data.Migrations
{
    public partial class AddedPublishStatusenum : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsPublished",
                table: "Courses");

            migrationBuilder.AddColumn<int>(
                name: "PublishStatus",
                table: "Courses",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PublishStatus",
                table: "Courses");

            migrationBuilder.AddColumn<bool>(
                name: "IsPublished",
                table: "Courses",
                nullable: false,
                defaultValue: false);
        }
    }
}
