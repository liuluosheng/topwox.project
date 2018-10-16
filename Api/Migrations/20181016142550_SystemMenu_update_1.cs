using Microsoft.EntityFrameworkCore.Migrations;

namespace Api.Migrations
{
    public partial class SystemMenu_update_1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Url",
                table: "SystemMenu",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AddColumn<bool>(
                name: "Root",
                table: "SystemMenu",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Root",
                table: "SystemMenu");

            migrationBuilder.AlterColumn<string>(
                name: "Url",
                table: "SystemMenu",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}
