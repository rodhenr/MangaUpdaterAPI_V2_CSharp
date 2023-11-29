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
            migrationBuilder.Sql("INSERT INTO Sources(Name,BaseUrl) VALUES('MangaDex','https://api.mangadex.org/manga/')");
            
            migrationBuilder.Sql("INSERT INTO Sources(Name,BaseUrl) VALUES('AsuraScans','https://asuratoon.com/manga/')");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE FROM Sources");
        }
    }
}
