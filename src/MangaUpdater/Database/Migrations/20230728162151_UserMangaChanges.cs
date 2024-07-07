using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MangaUpdater.Infrastructure.Migrations
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
                columns: columnsArray1);

            migrationBuilder.CreateIndex(
                name: "IX_UserMangas_MangaId_SourceId_LastChapter",
                table: "UserMangas",
                columns: columnsArray0);

            migrationBuilder.CreateIndex(
                name: "IX_UserMangas_SourceId",
                table: "UserMangas",
                column: "SourceId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserMangas_Chapters_MangaId_SourceId_LastChapter",
                table: "UserMangas",
                columns: columnsArray,
                principalTable: "Chapters",
                principalColumns: principalColumns,
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_UserMangas_Sources_SourceId",
                table: "UserMangas",
                column: "SourceId",
                principalTable: "Sources",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        private static readonly string[] columns = new[] { "MangaId", "UserId" };
        private static readonly string[] principalColumns = new[] { "MangaId", "SourceId", "Number" };
        private static readonly string[] columnsArray = new[] { "MangaId", "SourceId", "LastChapter" };
        private static readonly string[] columnsArray0 = new[] { "MangaId", "SourceId", "LastChapter" };
        private static readonly string[] columnsArray1 = new[] { "MangaId", "UserId", "SourceId", "ChapterId" };

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
                columns: columns);
        }
    }
}
