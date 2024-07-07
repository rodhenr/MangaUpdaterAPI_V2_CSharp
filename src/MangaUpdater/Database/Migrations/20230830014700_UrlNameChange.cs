using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MangaUpdater.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UrlNameChange : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "BaseURL",
                table: "Sources",
                newName: "BaseUrl");

            migrationBuilder.RenameColumn(
                name: "URL",
                table: "MangaSources",
                newName: "Url");

            migrationBuilder.RenameColumn(
                name: "CoverURL",
                table: "Mangas",
                newName: "CoverUrl");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "BaseUrl",
                table: "Sources",
                newName: "BaseURL");

            migrationBuilder.RenameColumn(
                name: "Url",
                table: "MangaSources",
                newName: "URL");

            migrationBuilder.RenameColumn(
                name: "CoverUrl",
                table: "Mangas",
                newName: "CoverURL");
        }
    }
}
