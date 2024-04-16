using MangaUpdater.Application.Services;
using MangaUpdater.Data.Entities;
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
    public async Task GetUniqueGenresId_Should_Return_List_Int()
    {
        // Arrange
        var listGenreId = new List<int> { 1, 2, 3, 4 };

        _repository
            .Setup(repo => repo.GetUniqueGenreIdListAsync())
            .ReturnsAsync(listGenreId);

        // Act
        var result = await _service.GetUniqueGenresId();

        // Assert
        _repository.Verify(repo => repo.GetUniqueGenreIdListAsync(), Times.Once);
        result.Should().BeEquivalentTo(listGenreId);
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