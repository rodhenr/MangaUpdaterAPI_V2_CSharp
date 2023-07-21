namespace MangaUpdaterAPI.Models;

using Microsoft.EntityFrameworkCore;

public class MangaUpdaterContext : DbContext
{
    public MangaUpdaterContext(DbContextOptions<MangaUpdaterContext> options) : base(options)
    {

    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        //optionsBuilder.UseSqlServer("Server=127.0.0.1,1433;Database=nome-banco-dados;User Id=sa;Password=;");
    }

    public DbSet<Chapter> Chapters { get; set; }
    public DbSet<Genre> Genres { get; set; }
    public DbSet<Manga> Mangas { get; set; }
    public DbSet<MangaGenre> MangaGenres { get; set; }
    public DbSet<MangaSource> MangaSources { get; set; }
    public DbSet<Source> Sources { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<UserManga> UserMangas { get; set; }
}