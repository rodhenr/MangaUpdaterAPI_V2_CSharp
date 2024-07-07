using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MangaUpdater.Database.Migrations
{
    /// <inheritdoc />
    public partial class AddIdEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "UserMangas",
                type: "int",
                nullable: false,
                defaultValue: 0).Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "MangaSources",
                type: "int",
                nullable: false,
                defaultValue: 0).Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "MangaGenres",
                type: "int",
                nullable: false,
                defaultValue: 0).Annotation("SqlServer:Identity", "1, 1");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Id",
                table: "UserMangas");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "MangaSources");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "MangaGenres");
        }
    }
}
