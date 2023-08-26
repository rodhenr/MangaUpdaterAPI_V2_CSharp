using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MangaUpdater.Infra.Data.Migrations
{
    /// <inheritdoc />
    public partial class ChapterUserMangaChanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserMangas_Chapters_MangaId_SourceId_LastChapter",
                table: "UserMangas");

            migrationBuilder.DropIndex(
                name: "IX_UserMangas_MangaId_SourceId_LastChapter",
                table: "UserMangas");

            migrationBuilder.DropColumn(
                name: "LastChapter",
                table: "UserMangas");

            migrationBuilder.RenameColumn(
                name: "ChapterId",
                table: "UserMangas",
                newName: "CurrentChapterId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CurrentChapterId",
                table: "UserMangas",
                newName: "ChapterId");

            migrationBuilder.AddColumn<float>(
                name: "LastChapter",
                table: "UserMangas",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.Sql("DELETE FROM UserMangas;");

            migrationBuilder.CreateIndex(
                name: "IX_UserMangas_MangaId_SourceId_LastChapter",
                table: "UserMangas",
                columns: new[] { "MangaId", "SourceId", "LastChapter" });

            migrationBuilder.AddForeignKey(
                name: "FK_UserMangas_Chapters_MangaId_SourceId_LastChapter",
                table: "UserMangas",
                columns: new[] { "MangaId", "SourceId", "LastChapter" },
                principalTable: "Chapters",
                principalColumns: new[] { "MangaId", "SourceId", "Number" },
                onDelete: ReferentialAction.NoAction);
        }
    }
}
