using MangaUpdater.Application.Services;
using MangaUpdater.Domain.Entities;
using MangaUpdater.Domain.Interfaces;

namespace MangaUpdater.Application.Tests;

public class MangaGenreServiceTests
{
    private readonly Mock<IMangaGenreRepository> _repository;
    private readonly MangaGenreService _service;

    public MangaGenreServiceTests()
    {
        _repository = new Mock<IMangaGenreRepository>();
        _service = new MangaGenreService(_repository.Object);
    } // TODO: Configure DI

    [Fact]
    public void BulkCreate_MangaGenres_Should_Call_BulkCreate_Method_In_Repository()
    {
        // Arrange
        var mangaGenres = new List<MangaGenre>
        {
            new() { Id = 1, MangaId = 1, GenreId = 1},
            new() { Id = 1, MangaId = 1, GenreId = 2 },
            new() { Id = 1, MangaId = 2, GenreId = 5 }
        };

        // Act
        _service.BulkCreate(mangaGenres);

        // Assert
        _repository.Verify(repo => repo.BulkCreate(mangaGenres), Times.Once);
    }
    
    [Fact]
    public async Task SaveChanges_Should_Invoke_SaveAsync()
    {
        // Act
        await _service.SaveChanges();

        // Assert
        _repository.Verify(repo => repo.SaveAsync(), Times.Once);
    }
}