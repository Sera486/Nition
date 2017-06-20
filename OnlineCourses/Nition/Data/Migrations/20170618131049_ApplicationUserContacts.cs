using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace OnlineCourses.Data.Migrations
{
    public partial class ApplicationUserContacts : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Contacts",
                table: "AspNetUsers",
                newName: "Twitter");

            migrationBuilder.AddColumn<string>(
                name: "Facebook",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Linkedin",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Skype",
                table: "AspNetUsers",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Facebook",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Linkedin",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Skype",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "Twitter",
                table: "AspNetUsers",
                newName: "Contacts");
        }
    }
}
