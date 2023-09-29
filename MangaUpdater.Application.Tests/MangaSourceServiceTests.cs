using MangaUpdater.Application.Services;
using MangaUpdater.Domain.Entities;
using MangaUpdater.Domain.Interfaces;

namespace MangaUpdater.Application.Tests;

public class MangaSourceServiceTests
{
    private readonly Mock<IMangaSourceRepository> _repository;
    private readonly MangaSourceService _service;

    public MangaSourceServiceTests()
    {
        _repository = new Mock<IMangaSourceRepository>();
        _service = new MangaSourceService(_repository.Object);
    }

    [Fact]
    public async Task Add_Should_Call_Create_Method_In_Repository()
    {
        // Arrange
        var manga = new MangaSource { MangaId = 1, SourceId = 1, Url = "http" };

        // Act
        _service.Add(manga);

        // Assert
        _repository.Verify(repo => repo.Create(manga), Times.Once);
    }

    [Fact]
    public async Task GetByMangaIdAndSourceId_Should_Return_MangaSource_From_Repository()
    {
        // Arrange
        var sampleMangaSource = new MangaSource() { MangaId = 1, SourceId = 1, Url = "http1" };

        _repository.Setup(repo => repo.GetByMangaIdAndSourceIdAsync(1, 1)).ReturnsAsync(sampleMangaSource);

        // Act
        var result = await _service.GetByMangaIdAndSourceId(1, 1);

        // Assert
        _repository.Verify(repo => repo.GetByMangaIdAndSourceIdAsync(1, 1), Times.Once);
        Assert.Equal(sampleMangaSource, result);
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