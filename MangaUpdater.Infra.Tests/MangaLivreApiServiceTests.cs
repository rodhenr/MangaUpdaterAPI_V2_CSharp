using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using MangaUpdater.Application.Models.External.MangaLivre;
using MangaUpdater.Domain.Exceptions;
using MangaUpdater.Infra.Data.ExternalServices.MangaLivre;
using Moq.Protected;

namespace MangaUpdater.Infra.Tests;

public class MangaLivreApiServiceTests
{
    public MangaLivreApiServiceTests()
    {
    }

    [Fact]
    public async Task GetChaptersAsync_With_Default_LastSavedChapter_Valid_Response_Returns_Chapters()
    {
        // Arrange
        const int mangaLivreSerieId = 1;

        var responseContent = new MangaLivreChaptersData
        {
            Chapters = new List<MangaLivreChapters>
            {
                new()
                {
                    ChapterName = "",
                    ChapterDate = "2023-07-01",
                    ChapterNumber = "1",
                    ReleaseList = new Dictionary<string, MangaLivreReleaseInfo>()
                },
                new()
                {
                    ChapterName = "",
                    ChapterDate = "2023-08-01",
                    ChapterNumber = "2",
                    ReleaseList = new Dictionary<string, MangaLivreReleaseInfo>()
                },
            }
        };

        var json = JsonSerializer.Serialize(responseContent);
        var jsonContent = new StringContent(json, Encoding.UTF8, "application/json");

        var handlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);
        var httpResponse = new HttpResponseMessage()
        {
            StatusCode = HttpStatusCode.OK,
            Content = jsonContent
        };

        handlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(httpResponse)
            .Verifiable();

        var httpClient = new HttpClient(handlerMock.Object)
        {
            BaseAddress = new Uri("https://www.testing.dev/")
        };

        var mockHttpClientFactory = new Mock<IHttpClientFactory>();

        mockHttpClientFactory
            .Setup(_ => _.CreateClient(""))
            .Returns(httpClient);

        var service = new MangaLivreApiService(mockHttpClientFactory.Object);

        // Act
        var result = await service.GetChaptersAsync(mangaLivreSerieId);

        // Assert
        Assert.IsType<List<MangaLivreChapters>>(result);
        Assert.NotEmpty(result);
        result.Should().HaveCount(2);
    }
    
    [Fact]
    public async Task GetChaptersAsync_With_LastSavecChapter_Valid_Response_Returns_Chapters()
    {
        // Arrange
        const int mangaLivreSerieId = 1;

        var responseContent = new MangaLivreChaptersData
        {
            Chapters = new List<MangaLivreChapters>
            {
                new()
                {
                    ChapterName = "",
                    ChapterDate = "2023-07-01",
                    ChapterNumber = "1",
                    ReleaseList = new Dictionary<string, MangaLivreReleaseInfo>()
                },
                new()
                {
                    ChapterName = "",
                    ChapterDate = "2023-08-01",
                    ChapterNumber = "2",
                    ReleaseList = new Dictionary<string, MangaLivreReleaseInfo>()
                },
            }
        };

        var json = JsonSerializer.Serialize(responseContent);
        var jsonContent = new StringContent(json, Encoding.UTF8, "application/json");

        var handlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);
        var httpResponse = new HttpResponseMessage()
        {
            StatusCode = HttpStatusCode.OK,
            Content = jsonContent
        };

        handlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(httpResponse)
            .Verifiable();

        var httpClient = new HttpClient(handlerMock.Object)
        {
            BaseAddress = new Uri("https://www.testing.dev/")
        };

        var mockHttpClientFactory = new Mock<IHttpClientFactory>();

        mockHttpClientFactory
            .Setup(_ => _.CreateClient(""))
            .Returns(httpClient);

        var service = new MangaLivreApiService(mockHttpClientFactory.Object);

        // Act
        var result = await service.GetChaptersAsync(mangaLivreSerieId, 1);

        // Assert
        Assert.IsType<List<MangaLivreChapters>>(result);
        Assert.NotEmpty(result);
        result.Should().HaveCount(1);
    }

    [Fact]
    public async Task GetChaptersAsync_Bad_Request_Response_Throws_Bad_Request_Exception()
    {
        // Arrange
        const int mangaLivreSerieId = 1;

        var httpClientFactoryMock = new Mock<IHttpClientFactory>();
        var httpClientHandler = new Mock<HttpMessageHandler>();

        httpClientHandler
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.BadRequest
            });

        var httpClient = new HttpClient(httpClientHandler.Object);
        httpClientFactoryMock
            .Setup(factory => factory.CreateClient(It.IsAny<string>()))
            .Returns(httpClient);

        var mangaLivreApiService = new MangaLivreApiService(httpClientFactoryMock.Object);

        // Act and Assert
        await Assert.ThrowsAsync<BadRequestException>(async () =>
        {
            await mangaLivreApiService.GetChaptersAsync(mangaLivreSerieId);
        });
    }

}