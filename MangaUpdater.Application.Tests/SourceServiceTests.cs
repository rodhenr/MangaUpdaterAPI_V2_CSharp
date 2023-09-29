using MangaUpdater.Application.Services;
using MangaUpdater.Domain.Entities;
using MangaUpdater.Domain.Interfaces;

namespace MangaUpdater.Application.Tests;

public class SourceServiceTests
{
    private readonly Mock<ISourceRepository> _repository;
    private readonly SourceService _service;

    public SourceServiceTests()
    {
        _repository = new Mock<ISourceRepository>();
        _service = new SourceService(_repository.Object);
    }

    [Fact]
    public async Task Get_Should_Return_Sources_From_Repository()
    {
        // Arrange
        var sampleSources = new List<Source>
        {
            new() { Id = 1, Name = "Source1", BaseUrl = "http" },
            new() { Id = 2, Name = "Source2", BaseUrl = "https" },
        };

        _repository.Setup(repo => repo.GetAsync()).ReturnsAsync(sampleSources);

        // Act
        var result = await _service.Get();

        // Assert
        _repository.Verify(repo => repo.GetAsync(), Times.Once);
        Assert.Equal(sampleSources, result);
    }

    [Fact]
    public async Task GetById_Should_Return_Last_Source()
    {
        // Arrange
        var expectedSource = new Source { Id = 1, Name = "Source1", BaseUrl = "http" };

        _repository.Setup(repo => repo.GetByIdAsync(1))
            .ReturnsAsync(expectedSource);

        // Act
        var result = await _service.GetById(1);

        // Assert
        _repository.Verify(repo => repo.GetByIdAsync(1), Times.Once);
        Assert.Equal(expectedSource, result);
    }
}