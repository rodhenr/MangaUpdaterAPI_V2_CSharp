using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MangaUpdater.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddAdminRole : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("INSERT INTO AspNetRoles(Id, Name, NormalizedName, ConcurrencyStamp) VALUES('1', 'Admin', 'ADMIN', '123456789')");
            migrationBuilder.Sql("INSERT INTO AspNetUserRoles(UserId, RoleId) SELECT TOP 1 Id, '1' FROM AspNetUsers ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE FROM AspNetUserRoles");
            migrationBuilder.Sql("DELETE FROM AspNetRoles WHERE Id = 1");
        }
    }
}
