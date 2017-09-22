using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Nition.Data.Migrations
{
    public partial class Info : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TextBlocks");

            migrationBuilder.DropTable(
                name: "VideoBlocks");

            migrationBuilder.CreateTable(
                name: "InfoBlocks",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Discriminator = table.Column<string>(nullable: false),
                    Order = table.Column<int>(nullable: false),
                    LessonID = table.Column<int>(nullable: true),
                    Text = table.Column<string>(nullable: true),
                    VideoURL = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InfoBlocks", x => x.ID);
                    table.ForeignKey(
                        name: "FK_InfoBlocks_Lessons_LessonID",
                        column: x => x.LessonID,
                        principalTable: "Lessons",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_InfoBlocks_LessonID",
                table: "InfoBlocks",
                column: "LessonID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InfoBlocks");

            migrationBuilder.CreateTable(
                name: "TextBlocks",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    LessonID = table.Column<int>(nullable: true),
                    Order = table.Column<int>(nullable: false),
                    Text = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TextBlocks", x => x.ID);
                    table.ForeignKey(
                        name: "FK_TextBlocks_Lessons_LessonID",
                        column: x => x.LessonID,
                        principalTable: "Lessons",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "VideoBlocks",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    LessonID = table.Column<int>(nullable: true),
                    Order = table.Column<int>(nullable: false),
                    VideoURL = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VideoBlocks", x => x.ID);
                    table.ForeignKey(
                        name: "FK_VideoBlocks_Lessons_LessonID",
                        column: x => x.LessonID,
                        principalTable: "Lessons",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TextBlocks_LessonID",
                table: "TextBlocks",
                column: "LessonID");

            migrationBuilder.CreateIndex(
                name: "IX_VideoBlocks_LessonID",
                table: "VideoBlocks",
                column: "LessonID");
        }
    }
}
