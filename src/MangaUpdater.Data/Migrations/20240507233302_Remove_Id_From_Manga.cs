using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MangaUpdater.Data.Migrations
{
    /// <inheritdoc />
    public partial class Remove_Id_From_Manga : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Chapters_Mangas_MangaId",
                table: "Chapters");

            migrationBuilder.DropForeignKey(
                name: "FK_MangaAuthors_Mangas_MangaId",
                table: "MangaAuthors");

            migrationBuilder.DropForeignKey(
                name: "FK_MangaGenres_Mangas_MangaId",
                table: "MangaGenres");

            migrationBuilder.DropForeignKey(
                name: "FK_MangaSources_Mangas_MangaId",
                table: "MangaSources");

            migrationBuilder.DropForeignKey(
                name: "FK_MangaTitles_Mangas_MangaId",
                table: "MangaTitles");

            migrationBuilder.DropForeignKey(
                name: "FK_UserMangas_Mangas_MangaId",
                table: "UserMangas");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Mangas",
                table: "Mangas");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Mangas");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Mangas",
                table: "Mangas",
                column: "MyAnimeListId");

            migrationBuilder.AddForeignKey(
                name: "FK_Chapters_Mangas_MangaId",
                table: "Chapters",
                column: "MangaId",
                principalTable: "Mangas",
                principalColumn: "MyAnimeListId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MangaAuthors_Mangas_MangaId",
                table: "MangaAuthors",
                column: "MangaId",
                principalTable: "Mangas",
                principalColumn: "MyAnimeListId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MangaGenres_Mangas_MangaId",
                table: "MangaGenres",
                column: "MangaId",
                principalTable: "Mangas",
                principalColumn: "MyAnimeListId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MangaSources_Mangas_MangaId",
                table: "MangaSources",
                column: "MangaId",
                principalTable: "Mangas",
                principalColumn: "MyAnimeListId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MangaTitles_Mangas_MangaId",
                table: "MangaTitles",
                column: "MangaId",
                principalTable: "Mangas",
                principalColumn: "MyAnimeListId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserMangas_Mangas_MangaId",
                table: "UserMangas",
                column: "MangaId",
                principalTable: "Mangas",
                principalColumn: "MyAnimeListId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Chapters_Mangas_MangaId",
                table: "Chapters");

            migrationBuilder.DropForeignKey(
                name: "FK_MangaAuthors_Mangas_MangaId",
                table: "MangaAuthors");

            migrationBuilder.DropForeignKey(
                name: "FK_MangaGenres_Mangas_MangaId",
                table: "MangaGenres");

            migrationBuilder.DropForeignKey(
                name: "FK_MangaSources_Mangas_MangaId",
                table: "MangaSources");

            migrationBuilder.DropForeignKey(
                name: "FK_MangaTitles_Mangas_MangaId",
                table: "MangaTitles");

            migrationBuilder.DropForeignKey(
                name: "FK_UserMangas_Mangas_MangaId",
                table: "UserMangas");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Mangas",
                table: "Mangas");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "Mangas",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Mangas",
                table: "Mangas",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Chapters_Mangas_MangaId",
                table: "Chapters",
                column: "MangaId",
                principalTable: "Mangas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MangaAuthors_Mangas_MangaId",
                table: "MangaAuthors",
                column: "MangaId",
                principalTable: "Mangas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MangaGenres_Mangas_MangaId",
                table: "MangaGenres",
                column: "MangaId",
                principalTable: "Mangas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MangaSources_Mangas_MangaId",
                table: "MangaSources",
                column: "MangaId",
                principalTable: "Mangas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MangaTitles_Mangas_MangaId",
                table: "MangaTitles",
                column: "MangaId",
                principalTable: "Mangas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserMangas_Mangas_MangaId",
                table: "UserMangas",
                column: "MangaId",
                principalTable: "Mangas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
