using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MangaUpdater.Database.Migrations
{
    /// <inheritdoc />
    public partial class MovedSourceIdToUserChapter : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserChapters_Chapters_ChapterId",
                table: "UserChapters");

            migrationBuilder.DropForeignKey(
                name: "FK_UserMangas_Sources_SourceId",
                table: "UserMangas");

            migrationBuilder.DropIndex(
                name: "IX_UserMangas_MangaId_UserId_SourceId",
                table: "UserMangas");

            migrationBuilder.DropIndex(
                name: "IX_UserMangas_SourceId",
                table: "UserMangas");

            migrationBuilder.DropIndex(
                name: "IX_UserChapters_ChapterId",
                table: "UserChapters");

            migrationBuilder.DropIndex(
                name: "IX_UserChapters_UserMangaId_ChapterId",
                table: "UserChapters");

            migrationBuilder.DropColumn(
                name: "SourceId",
                table: "UserMangas");

            migrationBuilder.AlterColumn<int>(
                name: "ChapterId",
                table: "UserChapters",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "SourceId",
                table: "UserChapters",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_UserMangas_MangaId_UserId",
                table: "UserMangas",
                columns: new[] { "MangaId", "UserId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserChapters_ChapterId",
                table: "UserChapters",
                column: "ChapterId",
                unique: true,
                filter: "[ChapterId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_UserChapters_SourceId",
                table: "UserChapters",
                column: "SourceId");

            migrationBuilder.CreateIndex(
                name: "IX_UserChapters_UserMangaId_SourceId",
                table: "UserChapters",
                columns: new[] { "UserMangaId", "SourceId" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_UserChapters_Chapters_ChapterId",
                table: "UserChapters",
                column: "ChapterId",
                principalTable: "Chapters",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserChapters_Sources_SourceId",
                table: "UserChapters",
                column: "SourceId",
                principalTable: "Sources",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserChapters_Chapters_ChapterId",
                table: "UserChapters");

            migrationBuilder.DropForeignKey(
                name: "FK_UserChapters_Sources_SourceId",
                table: "UserChapters");

            migrationBuilder.DropIndex(
                name: "IX_UserMangas_MangaId_UserId",
                table: "UserMangas");

            migrationBuilder.DropIndex(
                name: "IX_UserChapters_ChapterId",
                table: "UserChapters");

            migrationBuilder.DropIndex(
                name: "IX_UserChapters_SourceId",
                table: "UserChapters");

            migrationBuilder.DropIndex(
                name: "IX_UserChapters_UserMangaId_SourceId",
                table: "UserChapters");

            migrationBuilder.DropColumn(
                name: "SourceId",
                table: "UserChapters");

            migrationBuilder.AddColumn<int>(
                name: "SourceId",
                table: "UserMangas",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "ChapterId",
                table: "UserChapters",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserMangas_MangaId_UserId_SourceId",
                table: "UserMangas",
                columns: new[] { "MangaId", "UserId", "SourceId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserMangas_SourceId",
                table: "UserMangas",
                column: "SourceId");

            migrationBuilder.CreateIndex(
                name: "IX_UserChapters_ChapterId",
                table: "UserChapters",
                column: "ChapterId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserChapters_UserMangaId_ChapterId",
                table: "UserChapters",
                columns: new[] { "UserMangaId", "ChapterId" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_UserChapters_Chapters_ChapterId",
                table: "UserChapters",
                column: "ChapterId",
                principalTable: "Chapters",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserMangas_Sources_SourceId",
                table: "UserMangas",
                column: "SourceId",
                principalTable: "Sources",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
