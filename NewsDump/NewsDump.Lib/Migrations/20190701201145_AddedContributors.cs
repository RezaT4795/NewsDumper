using Microsoft.EntityFrameworkCore.Migrations;

namespace NewsDump.Lib.Migrations
{
    public partial class AddedContributors : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Contributors",
                table: "News",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Contributors",
                table: "News");
        }
    }
}
