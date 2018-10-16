using Microsoft.EntityFrameworkCore.Migrations;

namespace Api.Migrations
{
    public partial class SystemMenu_update : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "SystemMenu",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "SystemMenu");
        }
    }
}
