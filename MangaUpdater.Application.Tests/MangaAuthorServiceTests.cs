using MangaUpdater.Application.Services;
using MangaUpdater.Domain.Entities;
using MangaUpdater.Domain.Interfaces;

namespace MangaUpdater.Application.Tests;

public class MangaAuthorServiceTests
{
    private readonly Mock<IMangaAuthorRepository> _repository;
    private readonly MangaAuthorService _service;

    public MangaAuthorServiceTests()
    {
        _repository = new Mock<IMangaAuthorRepository>();
        _service = new MangaAuthorService(_repository.Object);
    }

    [Fact]
    public void BulkCreate_Should_Call_BulkCreate_Method_In_Repository()
    {
        // Act
        _service.BulkCreate(It.IsAny<List<MangaAuthor>>());

        // Assert
        _repository.Verify(repo => repo.BulkCreate(It.IsAny<List<MangaAuthor>>()), Times.Once);
    }
}