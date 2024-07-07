using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MangaUpdater.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Fixed_UserManga : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_UserChapters_UserMangaId",
                table: "UserChapters");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_UserChapters_UserMangaId",
                table: "UserChapters",
                column: "UserMangaId",
                unique: true);
        }
    }
}
