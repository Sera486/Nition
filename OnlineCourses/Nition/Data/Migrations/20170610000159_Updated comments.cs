using Microsoft.EntityFrameworkCore.Migrations;

namespace OnlineCourses.Data.Migrations
{
    public partial class Updatedcomments : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CourseID",
                table: "Comments",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Comments_CourseID",
                table: "Comments",
                column: "CourseID");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Courses_CourseID",
                table: "Comments",
                column: "CourseID",
                principalTable: "Courses",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Courses_CourseID",
                table: "Comments");

            migrationBuilder.DropIndex(
                name: "IX_Comments_CourseID",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "CourseID",
                table: "Comments");
        }
    }
}
