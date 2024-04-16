using MangaUpdater.Core.Common.Exceptions;
using MangaUpdater.Core.Common.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MangaUpdater.Infra.Tests;

public class ExceptionHandlerTests
{
    public ExceptionHandlerTests()
    {
    }

    [Fact]
    public void AuthExceptionHandler_Should_Handle_Exception()
    {
        // Arrange
        var exception = new AuthorizationException("Authorization failed");
        var exceptionHandler = new AuthExceptionHandler();

        // Act
        var result = exceptionHandler.CanHandle(exception);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void AuthExceptionHandler_Should_Return_Problem_Details()
    {
        // Arrange
        var exception = new AuthorizationException("Authorization failed");
        var exceptionHandler = new AuthExceptionHandler();
        var httpContextMock = new Mock<HttpContext>();
        httpContextMock.Setup(ctx => ctx.Response.StatusCode).Returns(StatusCodes.Status401Unauthorized);

        // Act
        var result = exceptionHandler.Handle(httpContextMock.Object, exception);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Unauthorized", result.Title);
        Assert.Equal(StatusCodes.Status401Unauthorized, result.Status);
        Assert.Equal("Authorization failed", result.Detail);
    }

    [Fact]
    public void NotFoundExceptionHandler_Should_Handle_Exception()
    {
        // Arrange
        var exception = new NotFoundException("Not found");
        var exceptionHandler = new NotFoundExceptionHandler();

        // Act
        var result = exceptionHandler.CanHandle(exception);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void NotFoundExceptionHandler_Should_Return_Problem_Details()
    {
        // Arrange
        var exception = new NotFoundException("Not found");
        var exceptionHandler = new NotFoundExceptionHandler();
        var httpContextMock = new Mock<HttpContext>();
        httpContextMock.Setup(ctx => ctx.Response.StatusCode).Returns(StatusCodes.Status404NotFound);

        // Act
        var result = exceptionHandler.Handle(httpContextMock.Object, exception);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Not Found", result.Title);
        Assert.Equal(StatusCodes.Status404NotFound, result.Status);
        Assert.Equal("Not found", result.Detail);
    }

    [Fact]
    public void ValidationExceptionHandler_Should_Handle_Exception()
    {
        // Arrange
        var exception = new ValidationException("Invalid");
        var exceptionHandler = new ValidationExceptionHandler();

        // Act
        var result = exceptionHandler.CanHandle(exception);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void ValidationExceptionHandler_Should_Return_Problem_Details()
    {
        // Arrange
        var exception = new ValidationException("Validations errors occured");
        var exceptionHandler = new ValidationExceptionHandler();
        var httpContextMock = new Mock<HttpContext>();
        httpContextMock.Setup(ctx => ctx.Response.StatusCode).Returns(StatusCodes.Status400BadRequest);

        // Act
        var result = exceptionHandler.Handle(httpContextMock.Object, exception);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Validations errors occured", result.Title);
        Assert.Equal(StatusCodes.Status400BadRequest, result.Status);
    }

    [Fact]
    public void BadRequestExceptionHandler_Should_Handle_Exception()
    {
        // Arrange
        var exception = new BadRequestException("Bad Request");
        var exceptionHandler = new BadRequestExceptionHandler();

        // Act
        var result = exceptionHandler.CanHandle(exception);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void BadRequestExceptionHandler_Should_Return_Problem_Details()
    {
        // Arrange
        var exception = new BadRequestException("Invalid Request");
        var exceptionHandler = new BadRequestExceptionHandler();
        var httpContextMock = new Mock<HttpContext>();
        httpContextMock.Setup(ctx => ctx.Response.StatusCode).Returns(StatusCodes.Status400BadRequest);

        // Act
        var result = exceptionHandler.Handle(httpContextMock.Object, exception);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Invalid Request", result.Title);
        Assert.Equal(StatusCodes.Status400BadRequest, result.Status);
    }
}