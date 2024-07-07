using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MangaUpdater.Database.Migrations
{
    /// <inheritdoc />
    public partial class AddUserChapter : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_UserMangas",
                table: "UserMangas");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Chapters",
                table: "Chapters");

            migrationBuilder.DropColumn(
                name: "CurrentChapterId",
                table: "UserMangas");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserMangas",
                table: "UserMangas",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Chapters",
                table: "Chapters",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "UserChapters",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserMangaId = table.Column<int>(type: "int", nullable: false),
                    ChapterId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserChapters", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserChapters_Chapters_ChapterId",
                        column: x => x.ChapterId,
                        principalTable: "Chapters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_UserChapters_UserMangas_UserMangaId",
                        column: x => x.UserMangaId,
                        principalTable: "UserMangas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserMangas_MangaId_UserId_SourceId",
                table: "UserMangas",
                columns: new[] { "MangaId", "UserId", "SourceId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Chapters_MangaId_SourceId_Number",
                table: "Chapters",
                columns: new[] { "MangaId", "SourceId", "Number" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserChapters_ChapterId",
                table: "UserChapters",
                column: "ChapterId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserChapters_UserMangaId",
                table: "UserChapters",
                column: "UserMangaId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserChapters_UserMangaId_ChapterId",
                table: "UserChapters",
                columns: new[] { "UserMangaId", "ChapterId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserChapters");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserMangas",
                table: "UserMangas");

            migrationBuilder.DropIndex(
                name: "IX_UserMangas_MangaId_UserId_SourceId",
                table: "UserMangas");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Chapters",
                table: "Chapters");

            migrationBuilder.DropIndex(
                name: "IX_Chapters_MangaId_SourceId_Number",
                table: "Chapters");

            migrationBuilder.AddColumn<int>(
                name: "CurrentChapterId",
                table: "UserMangas",
                type: "int",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserMangas",
                table: "UserMangas",
                columns: new[] { "MangaId", "UserId", "SourceId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_Chapters",
                table: "Chapters",
                columns: new[] { "MangaId", "SourceId", "Number" });
        }
    }
}
