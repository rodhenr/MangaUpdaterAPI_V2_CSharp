using MangaUpdater.Application.Services;
using MangaUpdater.Domain.Entities;
using MangaUpdater.Domain.Interfaces;

namespace MangaUpdater.Application.Tests;

public class MangaTitleServiceTests
{
    private readonly Mock<IMangaTitleRepository> _repository;
    private readonly MangaTitleService _service;

    public MangaTitleServiceTests()
    {
        _repository = new Mock<IMangaTitleRepository>();
        _service = new MangaTitleService(_repository.Object);
    }
    
    [Fact]
    public void BulkCreate_MangaTitles_Should_Call_BulkCreate_Method_In_Repository()
    {
        // Arrange
        var mangaTitles = new List<MangaTitle>
        {
            new() { Id = 1, Name = "Title1", MangaId = 1},
            new() { Id = 2, Name = "Title2", MangaId = 1},
            new() { Id = 3, Name = "Title1", MangaId = 2},
        };

        // Act
        _service.BulkCreate(mangaTitles);

        // Assert
        _repository.Verify(repo => repo.BulkCreate(mangaTitles), Times.Once);
    }
}