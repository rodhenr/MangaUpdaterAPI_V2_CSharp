using FluentValidation;
using FluentValidation.Results;
using MangaUpdater.Exceptions;
using MangaUpdater.Middlewares;
using NSubstitute;
using Microsoft.AspNetCore.Http;
using InvalidOperationException = System.InvalidOperationException;

namespace MangaUpdater.UnitTests.MiddlewaresTests;

public class ValidationExceptionHandlingMiddlewareTests
{
    private readonly ValidationExceptionHandlingMiddleware _middleware;
    
    public ValidationExceptionHandlingMiddlewareTests()
    {
        var next = Substitute.For<RequestDelegate>();
        _middleware = new ValidationExceptionHandlingMiddleware(next);
    }
    
    [Fact]
    public async Task InvokeAsync_ShouldReturn400BadRequest_WhenInvalidOperationExceptionThrown()
    {
        // Arrange
        var context = new DefaultHttpContext
        {
            Response =
            {
                Body = new MemoryStream()
            }
        };
        
        var exception = new InvalidOperationException("Invalid operation");
        var next = new RequestDelegate(_ => throw exception);
        var middleware = new ValidationExceptionHandlingMiddleware(next);

        // Act
        await middleware.InvokeAsync(context);

        // Assert
        context.Response.Body.Seek(0, SeekOrigin.Begin);
        
        var reader = new StreamReader(context.Response.Body);
        var responseBody = await reader.ReadToEndAsync();

        Assert.Equal(StatusCodes.Status400BadRequest, context.Response.StatusCode);
        Assert.Contains("Invalid operation", responseBody);
    }
    
    [Fact]
    public async Task InvokeAsync_ShouldReturn404NotFound_WhenEntityNotFoundExceptionThrown()
    {
        // Arrange
        var context = new DefaultHttpContext
        {
            Response =
            {
                Body = new MemoryStream()
            }
        };
        var exception = new EntityNotFoundException("Entity not found");
        var next = new RequestDelegate(_ => throw exception);
        var middleware = new ValidationExceptionHandlingMiddleware(next);

        // Act
        await middleware.InvokeAsync(context);

        // Assert
        context.Response.Body.Seek(0, SeekOrigin.Begin);
        
        var reader = new StreamReader(context.Response.Body);
        var responseBody = await reader.ReadToEndAsync();
        
        Assert.Equal(StatusCodes.Status404NotFound, context.Response.StatusCode);
        Assert.Contains("Entity not found", responseBody);
    }

    [Fact]
    public async Task InvokeAsync_ShouldReturn404NotFound_WhenNotFoundExceptionThrown()
    {
        // Arrange
        var context = new DefaultHttpContext
        {
            Response =
            {
                Body = new MemoryStream()
            }
        };
        var exception = new NotFoundException("Not found");
        var next = new RequestDelegate(_ => throw exception);
        var middleware = new ValidationExceptionHandlingMiddleware(next);

        // Act
        await middleware.InvokeAsync(context);

        // Assert
        context.Response.Body.Seek(0, SeekOrigin.Begin);
        
        var reader = new StreamReader(context.Response.Body);
        var responseBody = await reader.ReadToEndAsync();
        
        Assert.Equal(StatusCodes.Status404NotFound, context.Response.StatusCode);
        Assert.Contains("Not found", responseBody);
    }

    [Fact]
    public async Task InvokeAsync_ShouldReturn401Unauthorized_WhenAuthorizationExceptionThrown()
    {
        // Arrange
        var context = new DefaultHttpContext
        {
            Response =
            {
                Body = new MemoryStream()
            }
        };
        var exception = new AuthorizationException("Unauthorized");
        var next = new RequestDelegate(_ => throw exception);
        var middleware = new ValidationExceptionHandlingMiddleware(next);

        // Act
        await middleware.InvokeAsync(context);

        // Assert
        context.Response.Body.Seek(0, SeekOrigin.Begin);
        
        var reader = new StreamReader(context.Response.Body);
        var responseBody = await reader.ReadToEndAsync();
        
        Assert.Equal(StatusCodes.Status401Unauthorized, context.Response.StatusCode);
        Assert.Contains("Unauthorized", responseBody);
    }

    [Fact]
    public async Task InvokeAsync_ShouldReturn400BadRequest_WhenUserNotFoundExceptionThrown()
    {
        // Arrange
        var context = new DefaultHttpContext
        {
            Response =
            {
                Body = new MemoryStream()
            }
        };
        var exception = new UserNotFoundException("User not found");
        var next = new RequestDelegate(_ => throw exception);
        var middleware = new ValidationExceptionHandlingMiddleware(next);

        // Act
        await middleware.InvokeAsync(context);

        // Assert
        context.Response.Body.Seek(0, SeekOrigin.Begin);
        
        var reader = new StreamReader(context.Response.Body);
        var responseBody = await reader.ReadToEndAsync();
        
        Assert.Equal(StatusCodes.Status400BadRequest, context.Response.StatusCode);
        Assert.Contains("User not found", responseBody);
    }

    [Fact]
    public async Task InvokeAsync_ShouldReturn400BadRequest_WhenBadRequestExceptionThrown()
    {
        // Arrange
        var context = new DefaultHttpContext
        {
            Response =
            {
                Body = new MemoryStream()
            }
        };
        var exception = new BadRequestException("Bad request");       
        var next = new RequestDelegate(_ => throw exception);
        var middleware = new ValidationExceptionHandlingMiddleware(next);

        // Act
        await middleware.InvokeAsync(context);

        // Assert
        context.Response.Body.Seek(0, SeekOrigin.Begin);
        
        var reader = new StreamReader(context.Response.Body);
        var responseBody = await reader.ReadToEndAsync();
        
        Assert.Equal(StatusCodes.Status400BadRequest, context.Response.StatusCode);
        Assert.Contains("Bad request", responseBody);
    }
    
    [Fact]
    public async Task InvokeAsync_ShouldReturn400BadRequest_WithValidationErrors_WhenValidationExceptionThrown()
    {
        // Arrange
        var context = new DefaultHttpContext();
        context.Response.Body = new MemoryStream();

        var errors = new List<ValidationFailure>
        {
            new ValidationFailure("Property1", "Error message 1"),
            new ValidationFailure("Property2", "Error message 2")
        };
        var exception = new ValidationException(errors);
        var next = new RequestDelegate(_ => throw exception);
        var middleware = new ValidationExceptionHandlingMiddleware(next);

        // Act
        await middleware.InvokeAsync(context);

        // Assert
        context.Response.Body.Seek(0, SeekOrigin.Begin);
        var reader = new StreamReader(context.Response.Body);
        var responseBody = await reader.ReadToEndAsync();

        Assert.Equal(StatusCodes.Status400BadRequest, context.Response.StatusCode);
        Assert.Contains("One or more validation errors has occurred", responseBody);
        Assert.Contains("Property1", responseBody);
        Assert.Contains("Error message 1", responseBody);
        Assert.Contains("Property2", responseBody);
        Assert.Contains("Error message 2", responseBody);
    }
}