using Microsoft.EntityFrameworkCore;
using MangaUpdater.Domain.Entities;

namespace MangaUpdater.Infra.Context;

public class MangaUpdaterContext : DbContext
{
    public MangaUpdaterContext(DbContextOptions<MangaUpdaterContext> options) : base(options)
    {
    }

    public DbSet<Chapter> Chapters { get; set; }
    public DbSet<Genre> Genres { get; set; }
    public DbSet<Manga> Mangas { get; set; }
    public DbSet<MangaGenre> MangaGenres { get; set; }
    public DbSet<MangaSource> MangaSources { get; set; }
    public DbSet<Source> Sources { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<UserManga> UserMangas { get; set; }
    public DbSet<MangaRegister> MangaRegisters { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(typeof(MangaUpdaterContext).Assembly);
    }
}