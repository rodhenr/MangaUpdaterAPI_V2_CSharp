using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MangaUpdater.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ChapterNumberChange : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(name: "PK_Chapters", table: "Chapters");
            
            migrationBuilder.AlterColumn<string>(
                name: "Number",
                table: "Chapters",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(float),
                oldType: "real");
            
            migrationBuilder.AddPrimaryKey(
                name: "PK_Chapters",
                table: "Chapters",
                columns: new[] { "MangaId", "SourceId", "Number" }
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(name: "PK_Chapters", table: "Chapters");
            
            migrationBuilder.AlterColumn<float>(
                name: "Number",
                table: "Chapters",
                type: "real",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");
            
            migrationBuilder.AddPrimaryKey(
                name: "PK_Chapters",
                table: "Chapters",
                columns: new[] { "MangaId", "SourceId", "Number" }
            );
        }
    }
}
