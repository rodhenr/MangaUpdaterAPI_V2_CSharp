using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MangaUpdater.Infra.Data.Migrations
{
    /// <inheritdoc />
    public partial class UserMangaChanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_UserMangas",
                table: "UserMangas");

            migrationBuilder.AddColumn<int>(
                name: "SourceId",
                table: "UserMangas",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ChapterId",
                table: "UserMangas",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserMangas",
                table: "UserMangas",
                columns: new[] { "MangaId", "UserId", "SourceId", "ChapterId" });

            migrationBuilder.CreateIndex(
                name: "IX_UserMangas_MangaId_SourceId_LastChapter",
                table: "UserMangas",
                columns: new[] { "MangaId", "SourceId", "LastChapter" });

            migrationBuilder.CreateIndex(
                name: "IX_UserMangas_SourceId",
                table: "UserMangas",
                column: "SourceId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserMangas_Chapters_MangaId_SourceId_LastChapter",
                table: "UserMangas",
                columns: new[] { "MangaId", "SourceId", "LastChapter" },
                principalTable: "Chapters",
                principalColumns: new[] { "MangaId", "SourceId", "Number" },
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_UserMangas_Sources_SourceId",
                table: "UserMangas",
                column: "SourceId",
                principalTable: "Sources",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserMangas_Chapters_MangaId_SourceId_LastChapter",
                table: "UserMangas");

            migrationBuilder.DropForeignKey(
                name: "FK_UserMangas_Sources_SourceId",
                table: "UserMangas");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserMangas",
                table: "UserMangas");

            migrationBuilder.DropIndex(
                name: "IX_UserMangas_MangaId_SourceId_LastChapter",
                table: "UserMangas");

            migrationBuilder.DropIndex(
                name: "IX_UserMangas_SourceId",
                table: "UserMangas");

            migrationBuilder.DropColumn(
                name: "SourceId",
                table: "UserMangas");

            migrationBuilder.DropColumn(
                name: "ChapterId",
                table: "UserMangas");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserMangas",
                table: "UserMangas",
                columns: new[] { "MangaId", "UserId" });
        }
    }
}
