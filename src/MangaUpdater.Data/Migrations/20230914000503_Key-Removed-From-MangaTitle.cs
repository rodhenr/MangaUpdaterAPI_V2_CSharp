using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MangaUpdater.Data.Migrations
{
    /// <inheritdoc />
    public partial class KeyRemovedFromMangaTitle : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_MangaTitles",
                table: "MangaTitles");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "MangaTitles",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MangaTitles",
                table: "MangaTitles",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_MangaTitles_MangaId",
                table: "MangaTitles",
                column: "MangaId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_MangaTitles",
                table: "MangaTitles");

            migrationBuilder.DropIndex(
                name: "IX_MangaTitles_MangaId",
                table: "MangaTitles");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "MangaTitles",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MangaTitles",
                table: "MangaTitles",
                columns: new[] { "MangaId", "Name" });
        }
    }
}
