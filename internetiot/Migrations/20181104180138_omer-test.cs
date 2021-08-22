using Microsoft.EntityFrameworkCore.Migrations;

namespace internetiot.Migrations
{
    public partial class omertest : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Latitude",
                table: "EscapeRooms");

            migrationBuilder.DropColumn(
                name: "Longtitude",
                table: "EscapeRooms");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "Latitude",
                table: "EscapeRooms",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Longtitude",
                table: "EscapeRooms",
                nullable: false,
                defaultValue: 0.0);
        }
    }
}
