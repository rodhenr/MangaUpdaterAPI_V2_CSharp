using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using MangaUpdater.Domain.Entities;

namespace MangaUpdater.Infra.Data.Context;

public class IdentityMangaUpdaterContext : IdentityDbContext
{
    public IdentityMangaUpdaterContext(DbContextOptions<IdentityMangaUpdaterContext> options) : base(options)
    {
    }

    public DbSet<Chapter> Chapters { get; set; }
    public DbSet<Genre> Genres { get; set; }
    public DbSet<Manga> Mangas { get; set; }
    public DbSet<MangaAuthor> MangaAuthors { get; set; }
    public DbSet<MangaGenre> MangaGenres { get; set; }
    public DbSet<MangaSource> MangaSources { get; set; }
    public DbSet<Source> Sources { get; set; }
    public DbSet<UserManga> UserMangas { get; set; }
    public DbSet<MangaTitle> MangaTitles { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(typeof(IdentityMangaUpdaterContext).Assembly);
    }
}