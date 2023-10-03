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
    }

    [Fact]
    public void BulkCreate_Should_Call_BulkCreate_Method_In_Repository()
    {
        // Act
        _service.BulkCreate(It.IsAny<List<MangaGenre>>());

        // Assert
        _repository.Verify(repo => repo.BulkCreate(It.IsAny<List<MangaGenre>>()), Times.Once);
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