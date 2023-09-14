using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using MangaUpdater.Domain.Entities;

namespace MangaUpdater.Infra.Data.Context;

public class IdentityMangaUpdaterContext : IdentityDbContext
{
    public IdentityMangaUpdaterContext(DbContextOptions<IdentityMangaUpdaterContext> options) : base(options)
    {
    }

    public DbSet<Chapter> Chapters { get; set; } = null!;
    public DbSet<Genre> Genres { get; set; } = null!;
    public DbSet<Manga> Mangas { get; set; } = null!;
    public DbSet<MangaAuthor> MangaAuthors { get; set; } = null!;
    public DbSet<MangaGenre> MangaGenres { get; set; } = null!;
    public DbSet<MangaSource> MangaSources { get; set; } = null!;
    public DbSet<Source> Sources { get; set; } = null!;
    public DbSet<UserManga> UserMangas { get; set; } = null!;
    public DbSet<MangaTitle> MangaTitles { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(typeof(IdentityMangaUpdaterContext).Assembly);
    }
}