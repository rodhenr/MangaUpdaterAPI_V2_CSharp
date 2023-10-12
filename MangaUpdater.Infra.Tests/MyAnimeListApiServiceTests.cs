using System.Net;
using System.Text;
using System.Text.Json;
using Moq.Protected;
using MangaUpdater.Infra.Data.ExternalServices.MyAnimeList;
using MangaUpdater.Application.Models.External.MyAnimeList;
using MangaUpdater.Domain.Exceptions;

namespace MangaUpdater.Infra.Tests;

public class MyAnimeListApiServiceTests
{
    public MyAnimeListApiServiceTests()
    {
    }

    [Fact]
    public async Task GetMangaFromMyAnimeListByIdAsync_Valid_Response_Returns_MyAnimeListApiResponse()
    {
        // Arrange
        var responseContent = new MyAnimeListApiData
        {
            Data = new MyAnimeListApiResponse
            {
                Authors = new List<MalCollection>(),
                Genres = new List<MalCollection>(),
                Images = new ImagesSet
                {
                    JPG = new Image
                    {
                        LargeImageUrl = ""
                    }
                },
                Publishing = true,
                Status = "",
                Synopsis = "",
                Titles = new List<TitleEntry>(),
                Type = "",
                MalId = 1
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

        var service = new MyAnimeListApiService(mockHttpClientFactory.Object);

        // Act
        var result = await service.GetMangaFromMyAnimeListByIdAsync(1);

        // Assert
        Assert.NotNull(result);
        Assert.IsType<MyAnimeListApiResponse>(result);
    }

    [Fact]
    public async Task GetMangaFromMyAnimeListByIdAsync_Bad_Request_Response_Throws_Bad_Request_Exception()
    {
        // Arrange
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

        var service = new MyAnimeListApiService(httpClientFactoryMock.Object);

        // Act and Assert
        await Assert.ThrowsAsync<BadRequestException>(async () =>
        {
            await service.GetMangaFromMyAnimeListByIdAsync(1);
        });
    }
}