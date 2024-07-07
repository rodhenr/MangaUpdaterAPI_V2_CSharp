using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MangaUpdater.Database.Migrations
{
    /// <inheritdoc />
    public partial class AddMainTitle : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_MangaTitles_MangaId",
                table: "MangaTitles");

            migrationBuilder.AddColumn<bool>(
                name: "IsMainTitle",
                table: "MangaTitles",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_MangaTitles_MangaId_IsMainTitle",
                table: "MangaTitles",
                columns: new[] { "MangaId", "IsMainTitle" },
                unique: true,
                filter: "IsMainTitle = 1");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_MangaTitles_MangaId_IsMainTitle",
                table: "MangaTitles");

            migrationBuilder.DropColumn(
                name: "IsMainTitle",
                table: "MangaTitles");

            migrationBuilder.CreateIndex(
                name: "IX_MangaTitles_MangaId",
                table: "MangaTitles",
                column: "MangaId");
        }
    }
}
