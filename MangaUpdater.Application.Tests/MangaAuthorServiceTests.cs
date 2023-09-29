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
    public void BulkCreate_MangaAuthors_Should_Call_BulkCreate_Method_In_Repository()
    {
        // Arrange
        var mangaAuthors = new List<MangaAuthor>
        {
            new() { Id = 1, MangaId = 1, Name = "Author1" },
            new() { Id = 2, MangaId = 1, Name = "Author1" },  
            new() { Id = 3, MangaId = 2, Name = "Author1" }
        };

        // Act
        _service.BulkCreate(mangaAuthors);

        // Assert
        _repository.Verify(repo => repo.BulkCreate(mangaAuthors), Times.Once);
    }
}