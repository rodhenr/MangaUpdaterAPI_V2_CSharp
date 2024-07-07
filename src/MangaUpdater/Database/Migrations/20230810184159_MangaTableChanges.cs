using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MangaUpdater.Database.Migrations
{
    /// <inheritdoc />
    public partial class MangaTableChanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MyAnimeListURL",
                table: "Mangas");

            migrationBuilder.AddColumn<int>(
                name: "MyAnimeListId",
                table: "Mangas",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MyAnimeListId",
                table: "Mangas");

            migrationBuilder.AddColumn<string>(
                name: "MyAnimeListURL",
                table: "Mangas",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");
        }
    }
}
