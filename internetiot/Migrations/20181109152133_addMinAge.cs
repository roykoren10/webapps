using Microsoft.EntityFrameworkCore.Migrations;

namespace internetiot.Migrations
{
    public partial class addMinAge : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MinAge",
                table: "Genre",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MinAge",
                table: "Genre");
        }
    }
}
