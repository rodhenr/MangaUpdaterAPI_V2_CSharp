using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using MangaUpdater.Data.Entities;
using MangaUpdater.Data.Entities.Models;

namespace MangaUpdater.Data;

[RegisterScoped]
public class AppDbContextIdentity(DbContextOptions<AppDbContextIdentity> options)
    : IdentityDbContext<AppUser>(options)
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
    }
}