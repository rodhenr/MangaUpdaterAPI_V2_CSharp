using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MangaUpdater.Infra.Data.Migrations
{
    /// <inheritdoc />
    public partial class SeedDatabase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("INSERT INTO Sources(Name,BaseUrl) " +
                "VALUES('Manga Livre','https://mangalivre.net/serie/')");

            migrationBuilder.Sql("INSERT INTO Sources(Name,BaseUrl) " +
                "VALUES('Asura Scans','https://asuracomics.com/manga/')");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE FROM Sources");
        }
    }
}
