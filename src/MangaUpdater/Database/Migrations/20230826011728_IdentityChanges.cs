using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MangaUpdater.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class IdentityChanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserMangas_Users_UserId",
                table: "UserMangas");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropIndex(
                name: "IX_UserMangas_UserId",
                table: "UserMangas");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserMangas",
                table: "UserMangas");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "UserMangas",
                type: "nvarchar(450)",
                maxLength: 450,
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserMangas",
                table: "UserMangas",
                columns: new[] { "MangaId", "UserId", "SourceId" });

            migrationBuilder.AddForeignKey(
                name: "FK_UserMangas_AspNetUsers_UserId",
                table: "UserMangas",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_UserMangas",
                table: "UserMangas");

            migrationBuilder.DropForeignKey(
                name: "FK_UserMangas_AspNetUsers_UserId",
                table: "UserMangas");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "UserMangas",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldMaxLength: 450);

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Avatar = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserMangas",
                table: "UserMangas",
                columns: new[] { "MangaId", "UserId", "SourceId" });

            migrationBuilder.CreateIndex(
                name: "IX_UserMangas_UserId",
                table: "UserMangas",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserMangas_Users_UserId",
                table: "UserMangas",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
