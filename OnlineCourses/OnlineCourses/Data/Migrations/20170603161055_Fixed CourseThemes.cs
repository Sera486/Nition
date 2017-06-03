using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace OnlineCourses.Data.Migrations
{
    public partial class FixedCourseThemes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CourseThemes_Courses_ThemeID",
                table: "CourseThemes");

            migrationBuilder.AddForeignKey(
                name: "FK_CourseThemes_Courses_CourseID",
                table: "CourseThemes",
                column: "CourseID",
                principalTable: "Courses",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CourseThemes_Courses_CourseID",
                table: "CourseThemes");

            migrationBuilder.AddForeignKey(
                name: "FK_CourseThemes_Courses_ThemeID",
                table: "CourseThemes",
                column: "ThemeID",
                principalTable: "Courses",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
