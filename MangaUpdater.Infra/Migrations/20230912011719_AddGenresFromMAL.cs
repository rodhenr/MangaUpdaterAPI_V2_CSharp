using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MangaUpdater.Infra.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddGenresFromMAL : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(name: "FK_MangaGenres_Genres_GenreId", table: "MangaGenres");
            migrationBuilder.DropPrimaryKey(name: "PK_Genres", table: "Genres");
            migrationBuilder.DropColumn(name: "Id", table: "Genres");
            migrationBuilder.AddColumn<int>(name: "Id", table: "Genres", type: "int", nullable: false);
            migrationBuilder.AddPrimaryKey(name: "PK_Genres", table: "Genres", column: "Id");
            migrationBuilder.AddForeignKey(name: "FK_MangaGenres_Genres_GenreId", table: "MangaGenres", principalTable: "Genres", column: "GenreId", principalColumn: "Id");
            
            migrationBuilder.Sql("INSERT INTO Genres(Id, Name) VALUES(1, 'Action')");
            migrationBuilder.Sql("INSERT INTO Genres(Id, Name) VALUES(2, 'Adventure')");
            migrationBuilder.Sql("INSERT INTO Genres(Id, Name) VALUES(5, 'Avant Garde')");
            migrationBuilder.Sql("INSERT INTO Genres(Id, Name) VALUES(46, 'Award Winning')");
            migrationBuilder.Sql("INSERT INTO Genres(Id, Name) VALUES(28, 'Boys Love')");
            migrationBuilder.Sql("INSERT INTO Genres(Id, Name) VALUES(4, 'Comedy')");
            migrationBuilder.Sql("INSERT INTO Genres(Id, Name) VALUES(8, 'Drama')");
            migrationBuilder.Sql("INSERT INTO Genres(Id, Name) VALUES(10, 'Fantasy')");
            migrationBuilder.Sql("INSERT INTO Genres(Id, Name) VALUES(26, 'Girls Love')");
            migrationBuilder.Sql("INSERT INTO Genres(Id, Name) VALUES(47, 'Gourmet')");
            migrationBuilder.Sql("INSERT INTO Genres(Id, Name) VALUES(14, 'Horror')");
            migrationBuilder.Sql("INSERT INTO Genres(Id, Name) VALUES(7, 'Mystery')");
            migrationBuilder.Sql("INSERT INTO Genres(Id, Name) VALUES(22, 'Romance')");
            migrationBuilder.Sql("INSERT INTO Genres(Id, Name) VALUES(24, 'Sci-Fi')");
            migrationBuilder.Sql("INSERT INTO Genres(Id, Name) VALUES(36, 'Slice of Life')");
            migrationBuilder.Sql("INSERT INTO Genres(Id, Name) VALUES(30, 'Sports')");
            migrationBuilder.Sql("INSERT INTO Genres(Id, Name) VALUES(37, 'Supernatural')");
            migrationBuilder.Sql("INSERT INTO Genres(Id, Name) VALUES(45, 'Suspense')");
            migrationBuilder.Sql("INSERT INTO Genres(Id, Name) VALUES(9, 'Ecchi')");
            migrationBuilder.Sql("INSERT INTO Genres(Id, Name) VALUES(49, 'Erotica')");
            migrationBuilder.Sql("INSERT INTO Genres(Id, Name) VALUES(12, 'Hentai')");
            migrationBuilder.Sql("INSERT INTO Genres(Id, Name) VALUES(50, 'Adult Cast')");
            migrationBuilder.Sql("INSERT INTO Genres(Id, Name) VALUES(51, 'Anthropomorphic')");
            migrationBuilder.Sql("INSERT INTO Genres(Id, Name) VALUES(52, 'CGDCT')");
            migrationBuilder.Sql("INSERT INTO Genres(Id, Name) VALUES(53, 'Childcare')");
            migrationBuilder.Sql("INSERT INTO Genres(Id, Name) VALUES(54, 'Combat Sports')");
            migrationBuilder.Sql("INSERT INTO Genres(Id, Name) VALUES(44, 'Crossdressing')");
            migrationBuilder.Sql("INSERT INTO Genres(Id, Name) VALUES(55, 'Delinquents')");
            migrationBuilder.Sql("INSERT INTO Genres(Id, Name) VALUES(39, 'Detective')");
            migrationBuilder.Sql("INSERT INTO Genres(Id, Name) VALUES(56, 'Educational')");
            migrationBuilder.Sql("INSERT INTO Genres(Id, Name) VALUES(57, 'Gag Humor')");
            migrationBuilder.Sql("INSERT INTO Genres(Id, Name) VALUES(58, 'Gore')");
            migrationBuilder.Sql("INSERT INTO Genres(Id, Name) VALUES(35, 'Harem')");
            migrationBuilder.Sql("INSERT INTO Genres(Id, Name) VALUES(59, 'High Stakes Game')");
            migrationBuilder.Sql("INSERT INTO Genres(Id, Name) VALUES(13, 'Historical')");
            migrationBuilder.Sql("INSERT INTO Genres(Id, Name) VALUES(60, 'Idols (Female)')");
            migrationBuilder.Sql("INSERT INTO Genres(Id, Name) VALUES(61, 'Idols (Male)')");
            migrationBuilder.Sql("INSERT INTO Genres(Id, Name) VALUES(62, 'Isekai')");
            migrationBuilder.Sql("INSERT INTO Genres(Id, Name) VALUES(63, 'Iyashikei')");
            migrationBuilder.Sql("INSERT INTO Genres(Id, Name) VALUES(64, 'Love Polygon')");
            migrationBuilder.Sql("INSERT INTO Genres(Id, Name) VALUES(65, 'Magical Sex Shift')");
            migrationBuilder.Sql("INSERT INTO Genres(Id, Name) VALUES(66, 'Mahou Shoujo')");
            migrationBuilder.Sql("INSERT INTO Genres(Id, Name) VALUES(17, 'Martial Arts')");
            migrationBuilder.Sql("INSERT INTO Genres(Id, Name) VALUES(18, 'Mecha')");
            migrationBuilder.Sql("INSERT INTO Genres(Id, Name) VALUES(67, 'Medical')");
            migrationBuilder.Sql("INSERT INTO Genres(Id, Name) VALUES(68, 'Memoir')");
            migrationBuilder.Sql("INSERT INTO Genres(Id, Name) VALUES(38, 'Military')");
            migrationBuilder.Sql("INSERT INTO Genres(Id, Name) VALUES(19, 'Music')");
            migrationBuilder.Sql("INSERT INTO Genres(Id, Name) VALUES(6, 'Mythology')");
            migrationBuilder.Sql("INSERT INTO Genres(Id, Name) VALUES(69, 'Organized Crime')");
            migrationBuilder.Sql("INSERT INTO Genres(Id, Name) VALUES(70, 'Otaku Culture')");
            migrationBuilder.Sql("INSERT INTO Genres(Id, Name) VALUES(20, 'Parody')");
            migrationBuilder.Sql("INSERT INTO Genres(Id, Name) VALUES(71, 'Performing Arts')");
            migrationBuilder.Sql("INSERT INTO Genres(Id, Name) VALUES(72, 'Pets')");
            migrationBuilder.Sql("INSERT INTO Genres(Id, Name) VALUES(40, 'Psychological')");
            migrationBuilder.Sql("INSERT INTO Genres(Id, Name) VALUES(3, 'Racing')");
            migrationBuilder.Sql("INSERT INTO Genres(Id, Name) VALUES(73, 'Reincarnation')");
            migrationBuilder.Sql("INSERT INTO Genres(Id, Name) VALUES(74, 'Reverse Harem')");
            migrationBuilder.Sql("INSERT INTO Genres(Id, Name) VALUES(75, 'Romantic Subtext')");
            migrationBuilder.Sql("INSERT INTO Genres(Id, Name) VALUES(21, 'Samurai')");
            migrationBuilder.Sql("INSERT INTO Genres(Id, Name) VALUES(23, 'School')");
            migrationBuilder.Sql("INSERT INTO Genres(Id, Name) VALUES(76, 'Showbiz')");
            migrationBuilder.Sql("INSERT INTO Genres(Id, Name) VALUES(29, 'Space')");
            migrationBuilder.Sql("INSERT INTO Genres(Id, Name) VALUES(11, 'Strategy Game')");
            migrationBuilder.Sql("INSERT INTO Genres(Id, Name) VALUES(31, 'Super Power')");
            migrationBuilder.Sql("INSERT INTO Genres(Id, Name) VALUES(77, 'Survival')");
            migrationBuilder.Sql("INSERT INTO Genres(Id, Name) VALUES(78, 'Team Sports')");
            migrationBuilder.Sql("INSERT INTO Genres(Id, Name) VALUES(79, 'Time Travel')");
            migrationBuilder.Sql("INSERT INTO Genres(Id, Name) VALUES(32, 'Vampire')");
            migrationBuilder.Sql("INSERT INTO Genres(Id, Name) VALUES(80, 'Video Game')");
            migrationBuilder.Sql("INSERT INTO Genres(Id, Name) VALUES(81, 'Villainess')");
            migrationBuilder.Sql("INSERT INTO Genres(Id, Name) VALUES(82, 'Visual Arts')");
            migrationBuilder.Sql("INSERT INTO Genres(Id, Name) VALUES(48, 'Workplace')");
            migrationBuilder.Sql("INSERT INTO Genres(Id, Name) VALUES(42, 'Josei')");
            migrationBuilder.Sql("INSERT INTO Genres(Id, Name) VALUES(15, 'Kids')");
            migrationBuilder.Sql("INSERT INTO Genres(Id, Name) VALUES(41, 'Seinen')");
            migrationBuilder.Sql("INSERT INTO Genres(Id, Name) VALUES(25, 'Shoujo')");
            migrationBuilder.Sql("INSERT INTO Genres(Id, Name) VALUES(27, 'Shounen')");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(name: "FK_MangaGenres_Genres_GenreId", table: "MangaGenres");
            migrationBuilder.DropPrimaryKey(name: "PK_Genres", table: "Genres");
            migrationBuilder.Sql("DELETE FROM Genres");
            migrationBuilder.Sql("INSERT INTO Genres(Id, Name) VALUES(1, 'Action')");
            migrationBuilder.Sql("INSERT INTO Genres(Id, Name) VALUES(2, 'Adventure')");
            migrationBuilder.Sql("INSERT INTO Genres(Id, Name) VALUES(5, 'Avant Garde')");
            migrationBuilder.Sql("INSERT INTO Genres(Id, Name) VALUES(46, 'Award Winning')");
            migrationBuilder.Sql("INSERT INTO Genres(Id, Name) VALUES(28, 'Boys Love')");
            migrationBuilder.Sql("INSERT INTO Genres(Id, Name) VALUES(4, 'Comedy')");
            migrationBuilder.Sql("INSERT INTO Genres(Id, Name) VALUES(8, 'Drama')");
            migrationBuilder.Sql("INSERT INTO Genres(Id, Name) VALUES(10, 'Fantasy')");
            migrationBuilder.Sql("INSERT INTO Genres(Id, Name) VALUES(26, 'Girls Love')");
            migrationBuilder.Sql("INSERT INTO Genres(Id, Name) VALUES(47, 'Gourmet')");
            migrationBuilder.Sql("INSERT INTO Genres(Id, Name) VALUES(14, 'Horror')");
            migrationBuilder.Sql("INSERT INTO Genres(Id, Name) VALUES(7, 'Mystery')");
            migrationBuilder.Sql("INSERT INTO Genres(Id, Name) VALUES(22, 'Romance')");
            migrationBuilder.Sql("INSERT INTO Genres(Id, Name) VALUES(24, 'Sci-Fi')");
            migrationBuilder.Sql("INSERT INTO Genres(Id, Name) VALUES(36, 'Slice of Life')");
            migrationBuilder.Sql("INSERT INTO Genres(Id, Name) VALUES(30, 'Sports')");
            migrationBuilder.Sql("INSERT INTO Genres(Id, Name) VALUES(37, 'Supernatural')");
            migrationBuilder.Sql("INSERT INTO Genres(Id, Name) VALUES(45, 'Suspense')");
            migrationBuilder.Sql("INSERT INTO Genres(Id, Name) VALUES(9, 'Ecchi')");
            migrationBuilder.Sql("INSERT INTO Genres(Id, Name) VALUES(49, 'Erotica')");
            migrationBuilder.Sql("INSERT INTO Genres(Id, Name) VALUES(12, 'Hentai')");
            migrationBuilder.Sql("INSERT INTO Genres(Id, Name) VALUES(50, 'Adult Cast')");
            migrationBuilder.Sql("INSERT INTO Genres(Id, Name) VALUES(51, 'Anthropomorphic')");
            migrationBuilder.Sql("INSERT INTO Genres(Id, Name) VALUES(52, 'CGDCT')");
            migrationBuilder.Sql("INSERT INTO Genres(Id, Name) VALUES(53, 'Childcare')");
            migrationBuilder.Sql("INSERT INTO Genres(Id, Name) VALUES(54, 'Combat Sports')");
            migrationBuilder.Sql("INSERT INTO Genres(Id, Name) VALUES(44, 'Crossdressing')");
            migrationBuilder.Sql("INSERT INTO Genres(Id, Name) VALUES(55, 'Delinquents')");
            migrationBuilder.Sql("INSERT INTO Genres(Id, Name) VALUES(39, 'Detective')");
            migrationBuilder.Sql("INSERT INTO Genres(Id, Name) VALUES(56, 'Educational')");
            migrationBuilder.Sql("INSERT INTO Genres(Id, Name) VALUES(57, 'Gag Humor')");
            migrationBuilder.Sql("INSERT INTO Genres(Id, Name) VALUES(58, 'Gore')");
            migrationBuilder.Sql("INSERT INTO Genres(Id, Name) VALUES(35, 'Harem')");
            migrationBuilder.Sql("INSERT INTO Genres(Id, Name) VALUES(59, 'High Stakes Game')");
            migrationBuilder.Sql("INSERT INTO Genres(Id, Name) VALUES(13, 'Historical')");
            migrationBuilder.Sql("INSERT INTO Genres(Id, Name) VALUES(60, 'Idols (Female)')");
            migrationBuilder.Sql("INSERT INTO Genres(Id, Name) VALUES(61, 'Idols (Male)')");
            migrationBuilder.Sql("INSERT INTO Genres(Id, Name) VALUES(62, 'Isekai')");
            migrationBuilder.Sql("INSERT INTO Genres(Id, Name) VALUES(63, 'Iyashikei')");
            migrationBuilder.Sql("INSERT INTO Genres(Id, Name) VALUES(64, 'Love Polygon')");
            migrationBuilder.Sql("INSERT INTO Genres(Id, Name) VALUES(65, 'Magical Sex Shift')");
            migrationBuilder.Sql("INSERT INTO Genres(Id, Name) VALUES(66, 'Mahou Shoujo')");
            migrationBuilder.Sql("INSERT INTO Genres(Id, Name) VALUES(17, 'Martial Arts')");
            migrationBuilder.Sql("INSERT INTO Genres(Id, Name) VALUES(18, 'Mecha')");
            migrationBuilder.Sql("INSERT INTO Genres(Id, Name) VALUES(67, 'Medical')");
            migrationBuilder.Sql("INSERT INTO Genres(Id, Name) VALUES(68, 'Memoir')");
            migrationBuilder.Sql("INSERT INTO Genres(Id, Name) VALUES(38, 'Military')");
            migrationBuilder.Sql("INSERT INTO Genres(Id, Name) VALUES(19, 'Music')");
            migrationBuilder.Sql("INSERT INTO Genres(Id, Name) VALUES(6, 'Mythology')");
            migrationBuilder.Sql("INSERT INTO Genres(Id, Name) VALUES(69, 'Organized Crime')");
            migrationBuilder.Sql("INSERT INTO Genres(Id, Name) VALUES(70, 'Otaku Culture')");
            migrationBuilder.Sql("INSERT INTO Genres(Id, Name) VALUES(20, 'Parody')");
            migrationBuilder.Sql("INSERT INTO Genres(Id, Name) VALUES(71, 'Performing Arts')");
            migrationBuilder.Sql("INSERT INTO Genres(Id, Name) VALUES(72, 'Pets')");
            migrationBuilder.Sql("INSERT INTO Genres(Id, Name) VALUES(40, 'Psychological')");
            migrationBuilder.Sql("INSERT INTO Genres(Id, Name) VALUES(3, 'Racing')");
            migrationBuilder.Sql("INSERT INTO Genres(Id, Name) VALUES(73, 'Reincarnation')");
            migrationBuilder.Sql("INSERT INTO Genres(Id, Name) VALUES(74, 'Reverse Harem')");
            migrationBuilder.Sql("INSERT INTO Genres(Id, Name) VALUES(75, 'Romantic Subtext')");
            migrationBuilder.Sql("INSERT INTO Genres(Id, Name) VALUES(21, 'Samurai')");
            migrationBuilder.Sql("INSERT INTO Genres(Id, Name) VALUES(23, 'School')");
            migrationBuilder.Sql("INSERT INTO Genres(Id, Name) VALUES(76, 'Showbiz')");
            migrationBuilder.Sql("INSERT INTO Genres(Id, Name) VALUES(29, 'Space')");
            migrationBuilder.Sql("INSERT INTO Genres(Id, Name) VALUES(11, 'Strategy Game')");
            migrationBuilder.Sql("INSERT INTO Genres(Id, Name) VALUES(31, 'Super Power')");
            migrationBuilder.Sql("INSERT INTO Genres(Id, Name) VALUES(77, 'Survival')");
            migrationBuilder.Sql("INSERT INTO Genres(Id, Name) VALUES(78, 'Team Sports')");
            migrationBuilder.Sql("INSERT INTO Genres(Id, Name) VALUES(79, 'Time Travel')");
            migrationBuilder.Sql("INSERT INTO Genres(Id, Name) VALUES(32, 'Vampire')");
            migrationBuilder.Sql("INSERT INTO Genres(Id, Name) VALUES(80, 'Video Game')");
            migrationBuilder.Sql("INSERT INTO Genres(Id, Name) VALUES(81, 'Villainess')");
            migrationBuilder.Sql("INSERT INTO Genres(Id, Name) VALUES(82, 'Visual Arts')");
            migrationBuilder.Sql("INSERT INTO Genres(Id, Name) VALUES(48, 'Workplace')");
            migrationBuilder.Sql("INSERT INTO Genres(Id, Name) VALUES(42, 'Josei')");
            migrationBuilder.Sql("INSERT INTO Genres(Id, Name) VALUES(15, 'Kids')");
            migrationBuilder.Sql("INSERT INTO Genres(Id, Name) VALUES(41, 'Seinen')");
            migrationBuilder.Sql("INSERT INTO Genres(Id, Name) VALUES(25, 'Shoujo')");
            migrationBuilder.Sql("INSERT INTO Genres(Id, Name) VALUES(27, 'Shounen')");
            migrationBuilder.DropColumn(name: "Id", table: "Genres");
            migrationBuilder.AddColumn<int>(
                    name: "Id",
                    table: "Genres",
                    type: "int",
                    nullable: false)
                .Annotation("SqlServer:Identity", "1, 1");
            migrationBuilder.AddPrimaryKey(name: "PK_Genres", table: "Genres", column: "Id");
            migrationBuilder.AddForeignKey(name: "FK_MangaGenres_Genres_GenreId", table: "MangaGenres", principalTable: "Genres", column: "GenreId", principalColumn: "Id");
        }
    }
}
