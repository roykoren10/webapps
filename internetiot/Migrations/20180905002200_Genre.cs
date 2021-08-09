using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace internetiot.Migrations
{
    public partial class Genre : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "GenreId",
                table: "RageRooms",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Genre",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Genre", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RageRooms_GenreId",
                table: "RageRooms",
                column: "GenreId");

            migrationBuilder.AddForeignKey(
                name: "FK_RageRooms_Genre_GenreId",
                table: "RageRooms",
                column: "GenreId",
                principalTable: "Genre",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RageRooms_Genre_GenreId",
                table: "RageRooms");

            migrationBuilder.DropTable(
                name: "Genre");

            migrationBuilder.DropIndex(
                name: "IX_RageRooms_GenreId",
                table: "RageRooms");

            migrationBuilder.DropColumn(
                name: "GenreId",
                table: "RageRooms");
        }
    }
}
