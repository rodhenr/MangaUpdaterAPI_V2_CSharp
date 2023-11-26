using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MangaUpdater.Infra.Data.Migrations
{
    /// <inheritdoc />
    public partial class FixUserChapter : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_UserChapters_ChapterId",
                table: "UserChapters");

            migrationBuilder.CreateIndex(
                name: "IX_UserChapters_ChapterId",
                table: "UserChapters",
                column: "ChapterId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_UserChapters_ChapterId",
                table: "UserChapters");

            migrationBuilder.CreateIndex(
                name: "IX_UserChapters_ChapterId",
                table: "UserChapters",
                column: "ChapterId",
                unique: true,
                filter: "[ChapterId] IS NOT NULL");
        }
    }
}
