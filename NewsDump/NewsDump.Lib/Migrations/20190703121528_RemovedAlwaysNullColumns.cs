using Microsoft.EntityFrameworkCore.Migrations;

namespace NewsDump.Lib.Migrations
{
    public partial class RemovedAlwaysNullColumns : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Contributors",
                table: "News");

            migrationBuilder.DropColumn(
                name: "NewsSource",
                table: "News");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Contributors",
                table: "News",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NewsSource",
                table: "News",
                nullable: true);
        }
    }
}
