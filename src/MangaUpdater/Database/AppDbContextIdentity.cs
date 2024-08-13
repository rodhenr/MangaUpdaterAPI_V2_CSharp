using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using MangaUpdater.Entities;

namespace MangaUpdater.Database;

[RegisterScoped]
public class AppDbContextIdentity(DbContextOptions<AppDbContextIdentity> options) : IdentityDbContext<AppUser>(options)
{
    public DbSet<Chapter> Chapters { get; set; }
    public DbSet<Genre> Genres { get; set; }
    public DbSet<Manga> Mangas { get; set; }
    public DbSet<MangaAuthor> MangaAuthors { get; set; }
    public DbSet<MangaGenre> MangaGenres { get; set; }
    public DbSet<MangaSource> MangaSources { get; set; }
    public DbSet<MangaTitle> MangaTitles { get; set; }
    public DbSet<Source> Sources { get; set; }
    public DbSet<UserManga> UserMangas { get; set; }
    public DbSet<UserChapter> UserChapters { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(typeof(AppDbContextIdentity).Assembly);

        builder.Entity<Genre>().HasData(
            new Genre { Id = 1, Name = "Action" },
            new Genre { Id = 2, Name = "Adventure" },
            new Genre { Id = 3, Name = "Racing" },
            new Genre { Id = 4, Name = "Comedy" },
            new Genre { Id = 5, Name = "Avant Garde" },
            new Genre { Id = 6, Name = "Mythology" },
            new Genre { Id = 7, Name = "Mystery" },
            new Genre { Id = 8, Name = "Drama" },
            new Genre { Id = 9, Name = "Ecchi" },
            new Genre { Id = 10, Name = "Fantasy" },
            new Genre { Id = 11, Name = "Strategy Game" },
            new Genre { Id = 12, Name = "Hentai" },
            new Genre { Id = 13, Name = "Historical" },
            new Genre { Id = 14, Name = "Horror" },
            new Genre { Id = 15, Name = "Kids" },
            new Genre { Id = 17, Name = "Martial Arts" },
            new Genre { Id = 18, Name = "Mecha" },
            new Genre { Id = 19, Name = "Music" },
            new Genre { Id = 20, Name = "Parody" },
            new Genre { Id = 21, Name = "Samurai" },
            new Genre { Id = 22, Name = "Romance" },
            new Genre { Id = 23, Name = "School" },
            new Genre { Id = 24, Name = "Sci-Fi" },
            new Genre { Id = 25, Name = "Shoujo" },
            new Genre { Id = 26, Name = "Girls Love" },
            new Genre { Id = 27, Name = "Shounen" },
            new Genre { Id = 28, Name = "Boys Love" },
            new Genre { Id = 29, Name = "Space" },
            new Genre { Id = 30, Name = "Sports" },
            new Genre { Id = 31, Name = "Super Power" },
            new Genre { Id = 32, Name = "Vampire" },
            new Genre { Id = 35, Name = "Harem" },
            new Genre { Id = 36, Name = "Slice of Life" },
            new Genre { Id = 37, Name = "Supernatural" },
            new Genre { Id = 38, Name = "Military" },
            new Genre { Id = 39, Name = "Detective" },
            new Genre { Id = 40, Name = "Psychological" },
            new Genre { Id = 41, Name = "Seinen" },
            new Genre { Id = 42, Name = "Josei" },
            new Genre { Id = 44, Name = "Crossdressing" },
            new Genre { Id = 45, Name = "Suspense" },
            new Genre { Id = 46, Name = "Award Winning" },
            new Genre { Id = 47, Name = "Gourmet" },
            new Genre { Id = 48, Name = "Workplace" },
            new Genre { Id = 49, Name = "Erotica" },
            new Genre { Id = 50, Name = "Adult Cast" },
            new Genre { Id = 51, Name = "Anthropomorphic" },
            new Genre { Id = 52, Name = "CGDCT" },
            new Genre { Id = 53, Name = "Childcare" },
            new Genre { Id = 54, Name = "Combat Sports" },
            new Genre { Id = 55, Name = "Delinquents" },
            new Genre { Id = 56, Name = "Educational" },
            new Genre { Id = 57, Name = "Gag Humor" },
            new Genre { Id = 58, Name = "Gore" },
            new Genre { Id = 59, Name = "High Stakes Game" },
            new Genre { Id = 60, Name = "Idols (Female)" },
            new Genre { Id = 61, Name = "Idols (Male)" },
            new Genre { Id = 62, Name = "Isekai" },
            new Genre { Id = 63, Name = "Iyashikei" },
            new Genre { Id = 64, Name = "Love Polygon" },
            new Genre { Id = 65, Name = "Magical Sex Shift" },
            new Genre { Id = 66, Name = "Mahou Shoujo" },
            new Genre { Id = 67, Name = "Medical" },
            new Genre { Id = 68, Name = "Memoir" },
            new Genre { Id = 69, Name = "Organized Crime" },
            new Genre { Id = 70, Name = "Otaku Culture" },
            new Genre { Id = 71, Name = "Performing Arts" },
            new Genre { Id = 72, Name = "Pets" },
            new Genre { Id = 73, Name = "Reincarnation" },
            new Genre { Id = 74, Name = "Reverse Harem" },
            new Genre { Id = 75, Name = "Romantic Subtext" },
            new Genre { Id = 76, Name = "Showbiz" },
            new Genre { Id = 77, Name = "Survival" },
            new Genre { Id = 78, Name = "Team Sports" },
            new Genre { Id = 79, Name = "Time Travel" },
            new Genre { Id = 80, Name = "Video Game" },
            new Genre { Id = 81, Name = "Villainess" },
            new Genre { Id = 82, Name = "Visual Arts" }
        );

        builder.Entity<Source>().HasData(
            new Source { Id = 1, Name = "MangaDex", BaseUrl = "https://api.mangadex.org/manga/" },
            new Source { Id = 2, Name = "AsuraScans", BaseUrl = "https://asuracomic.net/series/" }
        );
    }
}