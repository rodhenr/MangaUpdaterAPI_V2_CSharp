using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MangaUpdater.Database.Scaffolding
{
    /// <inheritdoc />
    public partial class Add_MangaTitle_Column : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_MangaTitles_MangaId_IsMainTitle",
                table: "MangaTitles");
            
            migrationBuilder.RenameColumn(
                name: "IsMainTitle",
                table: "MangaTitles",
                newName: "IsMyAnimeListMainTitle");
            
            migrationBuilder.CreateIndex(
                name: "IX_MangaTitles_MangaId_IsMyAnimeListMainTitle",
                table: "MangaTitles",
                columns: new[] { "MangaId", "IsMyAnimeListMainTitle" },
                unique: true,
                filter: "IsMyAnimeListMainTitle = 1");

            migrationBuilder.AddColumn<bool>(
                name: "IsAsuraMainTitle",
                table: "MangaTitles",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<string>(
                name: "Avatar",
                table: "AspNetUsers",
                type: "nvarchar(MAX)",
                unicode: false,
                maxLength: 2147483647,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_MangaTitles_MangaId_IsMyAnimeListMainTitle",
                table: "MangaTitles");
            
            migrationBuilder.DropColumn(
                name: "IsAsuraMainTitle",
                table: "MangaTitles");

            migrationBuilder.RenameColumn(
                name: "IsMyAnimeListMainTitle",
                table: "MangaTitles",
                newName: "IsMainTitle");

            migrationBuilder.AlterColumn<string>(
                name: "Avatar",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(MAX)",
                oldUnicode: false,
                oldMaxLength: 2147483647);
            
            migrationBuilder.CreateIndex(
                name: "IX_MangaTitles_MangaId_IsMainTitle",
                table: "MangaTitles",
                columns: new[] { "MangaId", "IsMainTitle" },
                unique: true,
                filter: "IsMainTitle = 1");
        }
    }
}
