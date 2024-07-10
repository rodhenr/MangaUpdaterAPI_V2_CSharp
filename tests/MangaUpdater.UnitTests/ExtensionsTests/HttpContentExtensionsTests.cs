using System.Text.Json;
using MangaUpdater.Extensions;

namespace MangaUpdater.UnitTests.ExtensionsTests;

public record TestClass(string Name, int Value);

public class HttpContentExtensionsTests
{
    [Fact]
    public async Task TryToReadJsonAsync_ValidJson_ReturnsObject()
    {
        // Arrange
        var expected = new TestClass("SomeName", 1);
        var httpContent = new StringContent(JsonSerializer.Serialize(expected));

        // Act
        var result = await httpContent.TryToReadJsonAsync<TestClass>();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expected.Name, result.Name);
        Assert.Equal(expected.Value, result.Value);
    }

    [Fact]
    public async Task TryToReadJsonAsync_InvalidJson_ReturnsNull()
    {
        // Arrange
        var httpContent = new StringContent("invalid json");

        // Act
        var result = await httpContent.TryToReadJsonAsync<TestClass>();

        // Assert
        Assert.Null(result);
    }
}