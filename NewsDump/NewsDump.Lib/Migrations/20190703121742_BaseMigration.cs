using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace NewsDump.Lib.Migrations
{
    public partial class BaseMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "News",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Link = table.Column<string>(nullable: true),
                    NewsTitle = table.Column<string>(nullable: true),
                    NewsIntro = table.Column<string>(nullable: true),
                    NewsBody = table.Column<string>(nullable: true),
                    PublishDate = table.Column<DateTime>(nullable: false),
                    SiteName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_News", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "News");
        }
    }
}
