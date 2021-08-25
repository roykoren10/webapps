using Microsoft.EntityFrameworkCore.Migrations;

namespace internetiot.Migrations
{
    public partial class omertest : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Latitude",
                table: "RageRooms");

            migrationBuilder.DropColumn(
                name: "Longtitude",
                table: "RageRooms");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "Latitude",
                table: "RageRooms",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Longtitude",
                table: "RageRooms",
                nullable: false,
                defaultValue: 0.0);
        }
    }
}
