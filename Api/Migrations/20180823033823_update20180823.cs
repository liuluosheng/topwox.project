using Microsoft.EntityFrameworkCore.Migrations;

namespace Ew.Api.Migrations
{
    public partial class update20180823 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "position",
                table: "Users",
                newName: "Position");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Position",
                table: "Users",
                newName: "position");
        }
    }
}
