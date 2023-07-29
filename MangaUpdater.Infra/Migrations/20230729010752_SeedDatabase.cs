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
            migrationBuilder.Sql("INSERT INTO Users(Name,Email,Avatar) " +
            "VALUES('First User', 'someemail@email.com', 'http://someavatar.png')");

            migrationBuilder.Sql("INSERT INTO Sources(Name,BaseUrl) " +
                "VALUES('Source1','http://somesite.com/')");

            migrationBuilder.Sql("INSERT INTO Sources(Name,BaseUrl) " +
                "VALUES('Source2','http://anothersite.com/')");

            migrationBuilder.Sql("INSERT INTO Sources(Name,BaseUrl) " +
                "VALUES('Source3','http://site.com/')");

            migrationBuilder.Sql("INSERT INTO Sources(Name,BaseUrl) " +
                "VALUES('Source3','http://anwebsite.com/')");

            migrationBuilder.Sql("INSERT INTO Genres(Name) " +
                "VALUES('Action')");

            migrationBuilder.Sql("INSERT INTO Genres(Name) " +
                "VALUES('Isekai')");

            migrationBuilder.Sql("INSERT INTO Genres(Name) " +
                "VALUES('Adventure')");

            migrationBuilder.Sql("INSERT INTO Genres(Name) " +
                "VALUES('Fantasy')");

            migrationBuilder.Sql("INSERT INTO Mangas(CoverURL,Name,AlternativeName,Author,Synopsis,Type,MyAnimeListURL) " +
                "VALUES('https://cdn.myanimelist.net/images/manga/2/253146.jpg','One Piece','ワンピース','Eiichiro ODA','Some pirates...','Manga','https://myanimelist.net/manga/13/One_Piece')");

            migrationBuilder.Sql("INSERT INTO Mangas(CoverURL,Name,AlternativeName,Author,Synopsis,Type,MyAnimeListURL) " +
                "VALUES('https://cdn.myanimelist.net/images/manga/3/80661.jpg','One Punch Man','ワンパンマン','Yuusuke MURATA','Some monsters...','Manga','https://myanimelist.net/manga/44347/One_Punch-Man')");

            migrationBuilder.Sql("INSERT INTO Mangas(CoverURL,Name,AlternativeName,Author,Synopsis,Type,MyAnimeListURL) " +
                "VALUES('https://cdn.myanimelist.net/images/manga/3/167639.jpg','Tensei Shitara Slime Datta Ken','That Time I Got Reincarnated as a Slime','FUSE','He had been reincarnated into a slime!','Manga','https://myanimelist.net/manga/87609/Tensei_shitara_Slime_Datta_Ken')");

            migrationBuilder.Sql("INSERT INTO MangaSources(MangaId,SourceId,URL) " +
                "VALUES(1,1,'id/1234567')");

            migrationBuilder.Sql("INSERT INTO MangaSources(MangaId,SourceId,URL) " +
                "VALUES(1,3,'id/1234567')");

            migrationBuilder.Sql("INSERT INTO MangaSources(MangaId,SourceId,URL) " +
                "VALUES(1,4,'id/1234567')");

            migrationBuilder.Sql("INSERT INTO MangaSources(MangaId,SourceId,URL) " +
                "VALUES(2,2,'id/123456')");

            migrationBuilder.Sql("INSERT INTO MangaSources(MangaId,SourceId,URL) " +
               "VALUES(2,3,'id/123456')");

            migrationBuilder.Sql("INSERT INTO MangaGenres(MangaId,GenreId) " +
                "VALUES(1,1)");

            migrationBuilder.Sql("INSERT INTO MangaGenres(MangaId,GenreId) " +
                "VALUES(1,3)");

            migrationBuilder.Sql("INSERT INTO MangaGenres(MangaId,GenreId) " +
                "VALUES(1,4)");

            migrationBuilder.Sql("INSERT INTO MangaGenres(MangaId,GenreId) " +
                "VALUES(2,1)");

            migrationBuilder.Sql("INSERT INTO MangaGenres(MangaId,GenreId) " +
                "VALUES(2,3)");

            migrationBuilder.Sql("INSERT INTO Chapters(MangaId,SourceId,Date,Number) " +
                "VALUES(1,1,'2023-01-01',998)");

            migrationBuilder.Sql("INSERT INTO Chapters(MangaId,SourceId,Date,Number) " +
                "VALUES(1,1,'2023-02-01',999)");

            migrationBuilder.Sql("INSERT INTO Chapters(MangaId,SourceId,Date,Number) " +
                "VALUES(1,1,'2023-03-01',1000)");

            migrationBuilder.Sql("INSERT INTO Chapters(MangaId,SourceId,Date,Number) " +
                "VALUES(1,1,'2023-04-01',1001)");

            migrationBuilder.Sql("INSERT INTO Chapters(MangaId,SourceId,Date,Number) " +
                "VALUES(2,2,'2023-05-10',135)");

            migrationBuilder.Sql("INSERT INTO Chapters(MangaId,SourceId,Date,Number) " +
                "VALUES(2,2,'2023-05-20',136)");

            migrationBuilder.Sql("INSERT INTO Chapters(MangaId,SourceId,Date,Number) " +
                "VALUES(2,2,'2023-05-30',137)");

            migrationBuilder.Sql("INSERT INTO UserMangas(UserId,MangaId,SourceId,LastChapter,ChapterId) " +
                "VALUES(1,1,1,998,1)");

            migrationBuilder.Sql("INSERT INTO UserMangas(UserId,MangaId,SourceId,LastChapter,ChapterId) " +
                "VALUES(1,2,2,135,5)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE FROM UserMangas");
            migrationBuilder.Sql("DELETE FROM Chapters");
            migrationBuilder.Sql("DELETE FROM MangaGenres");
            migrationBuilder.Sql("DELETE FROM MangaSources");
            migrationBuilder.Sql("DELETE FROM Mangas");
            migrationBuilder.Sql("DELETE FROM Genres");
            migrationBuilder.Sql("DELETE FROM Sources");
            migrationBuilder.Sql("DELETE FROM Users");
        }
    }
}