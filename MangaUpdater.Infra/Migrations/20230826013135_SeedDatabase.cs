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
                "VALUES('Manga Livre','https://mangalivre.com/manga/')");

            migrationBuilder.Sql("INSERT INTO Sources(Name,BaseUrl) " +
                "VALUES('Asura Scans','https://asura.nacm.xyz/manga/')");

            migrationBuilder.Sql("INSERT INTO Genres(Name) " +
                "VALUES('Action')");

            migrationBuilder.Sql("INSERT INTO Genres(Name) " +
                "VALUES('Isekai')");

            migrationBuilder.Sql("INSERT INTO Genres(Name) " +
                "VALUES('Adventure')");

            migrationBuilder.Sql("INSERT INTO Genres(Name) " +
                "VALUES('Fantasy')");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE FROM Genres");
            migrationBuilder.Sql("DELETE FROM Sources");
        }
    }
}
