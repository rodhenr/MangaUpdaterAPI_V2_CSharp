using Microsoft.AspNetCore.Mvc;
using MangaUpdater.API.Controllers;
using MangaUpdater.Application.Interfaces;
using MangaUpdater.Domain.Entities;

namespace MangaUpdater.API.Tests;

public class SourceControllerTests
{
    private readonly Mock<ISourceService> _sourceService;
    private readonly SourceController _sourceController;

    public SourceControllerTests()
    {
        _sourceService = new Mock<ISourceService>();
        _sourceController = new SourceController(_sourceService.Object);
    }

    [Fact]
    public async Task GetSources_Should_Return_Ok_Result_With_All_Sources()
    {
        // Arrange
        var sourceList = new List<Source>
        {
            new() { Id = 1, Name = "Source1", BaseUrl = "url" },
            new() { Id = 2, Name = "Source2", BaseUrl = "url" },
            new() { Id = 3, Name = "Source3", BaseUrl = "url" },
            new() { Id = 4, Name = "Source4", BaseUrl = "url" }
        };

        _sourceService
            .Setup(s => s.Get())
            .ReturnsAsync(sourceList);

        // Act
        var result = await _sourceController.GetSources();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var response = Assert.IsType<List<Source>>(okResult.Value);
        response.Should().HaveCount(4);
    }

    [Fact]
    public async Task GetSourceById_Should_Return_Ok_Result_With_Source_If_Exists()
    {
        // Arrange
        var expectedSource = new Source { Id = 1, Name = "Source1", BaseUrl = "url" };
        var sourceList = new List<Source>
        {
            expectedSource,
            new() { Id = 2, Name = "Source2", BaseUrl = "url" },
            new() { Id = 3, Name = "Source3", BaseUrl = "url" },
            new() { Id = 4, Name = "Source4", BaseUrl = "url" }
        };

        _sourceService
            .Setup(s => s.GetById(1))
            .ReturnsAsync(expectedSource);

        // Act
        var result = await _sourceController.GetSourceById(1);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var response = Assert.IsType<Source>(okResult.Value);
        response.Should().BeEquivalentTo(expectedSource);
    }
}