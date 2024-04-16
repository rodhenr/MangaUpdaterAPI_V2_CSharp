using MangaUpdater.Application.Services;
using MangaUpdater.Data.Entities;
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
    public void BulkCreate_Should_Call_BulkCreate_Method_In_Repository()
    {
        // Act
        _service.BulkCreate(It.IsAny<List<MangaTitle>>());

        // Assert
        _repository.Verify(repo => repo.BulkCreate(It.IsAny<List<MangaTitle>>()), Times.Once);
    }
}