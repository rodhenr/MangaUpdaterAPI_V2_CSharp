using MangaUpdater.Application.Interfaces;
using MangaUpdater.Application.Interfaces.External.MangaLivre;
using MangaUpdater.Application.Models.External.MangaLivre;
using MangaUpdater.Application.Services.External.MangaLivre;
using MangaUpdater.Domain.Entities;

namespace MangaUpdater.Application.Tests;

public class MangaLivreServiceTests
{
    private readonly Mock<IMangaLivreApi> _mangaLivreApi;
    private readonly Mock<IChapterService> _chapterService;
    private readonly Mock<IMangaSourceService> _mangaSourceService;
    private readonly MangaLivreService _service;

    public MangaLivreServiceTests()
    {
        _mangaLivreApi = new Mock<IMangaLivreApi>();
        _chapterService = new Mock<IChapterService>();
        _mangaSourceService = new Mock<IMangaSourceService>();

        _service = new MangaLivreService(_mangaLivreApi.Object, _chapterService.Object, _mangaSourceService.Object);
    }

    [Fact]
    public async Task RegisterSourceAndChapters_ShouldAddMangaSourceAndUpdateChapters()
    {
        // Arrange
        _mangaLivreApi
            .Setup(mock => mock.GetChaptersAsync(1, 0))
            .ReturnsAsync(new List<MangaLivreChapters>());

        // Act
        await _service.RegisterSourceAndChapters(1, 1, "1");

        // Assert 
        _mangaSourceService.Verify(mock => mock.Add(It.IsAny<MangaSource>()), Times.Once);
        _mangaLivreApi.Verify(mock => mock.GetChaptersAsync(1, 0), Times.Once);
        _chapterService.Verify(mock => mock.BulkCreate(It.IsAny<List<Chapter>>()), Times.Once);
        _mangaSourceService.Verify(mock => mock.SaveChanges(), Times.Once);
    }

    [Fact]
    public async Task UpdateChapters_ShouldFetchChaptersBulkCreateAndSaveChanges()
    {
        // Arrange
        var sampleMangaLivreChapters = new List<MangaLivreChapters>
        {
            new()
            {
                ChapterName = "Chapter1",
                ChapterNumber = "1",
                ChapterDate = "2023-01-01",
                ReleaseList = new Dictionary<string, MangaLivreReleaseInfo>()
            },
            new()
            {
                ChapterName = "Chapter2",
                ChapterNumber = "2",
                ChapterDate = "2023-02-01",
                ReleaseList = new Dictionary<string, MangaLivreReleaseInfo>()
            },
            new()
            {
                ChapterName = "Chapter3",
                ChapterNumber = "3",
                ChapterDate = "2023-03-01",
                ReleaseList = new Dictionary<string, MangaLivreReleaseInfo>()
            },
        };

        _mangaLivreApi
            .Setup(mock => mock.GetChaptersAsync(1, 0))
            .ReturnsAsync(sampleMangaLivreChapters);

        _mangaSourceService
            .Setup(mock => mock.SaveChanges())
            .Returns(Task.CompletedTask);

        // Act
        await _service.UpdateChapters(1, 1, 0, "1");

        // Assert
        _mangaLivreApi.Verify(mock => mock.GetChaptersAsync(1, 0), Times.Once);
        _chapterService.Verify(mock => mock.BulkCreate(It.Is<List<Chapter>>(chapters =>
            chapters.Count() == sampleMangaLivreChapters.Count &&
            chapters.All(ch => ch.MangaId == 1 && ch.SourceId == 1)
        )), Times.Once);
        _mangaSourceService.Verify(service => service.SaveChanges(), Times.Once);
    }
}