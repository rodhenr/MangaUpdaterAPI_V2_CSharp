using MangaUpdater.Application.Services;
using MangaUpdater.Domain.Entities;
using MangaUpdater.Domain.Interfaces;

namespace MangaUpdater.Application.Tests;

public class GenreServiceTests
{
    private readonly Mock<IGenreRepository> _repository;
    private readonly GenreService _service;

    public GenreServiceTests()
    {
        _repository = new Mock<IGenreRepository>();
        _service = new GenreService(_repository.Object);
    }

    [Fact]
    public async Task Get_Should_Return_Genres_From_Repository()
    {
        // Arrange
        var sampleGenres = new List<Genre>
        {
            new() { Id = 1, Name = "Action" },
            new() { Id = 2, Name = "Comedy" },
        };

        _repository.Setup(repo => repo.GetAsync()).ReturnsAsync(sampleGenres);

        // Act
        var result = await _service.Get();

        // Assert
        _repository.Verify(repo => repo.GetAsync(), Times.Once);
        Assert.Equal(sampleGenres, result);
    }
}