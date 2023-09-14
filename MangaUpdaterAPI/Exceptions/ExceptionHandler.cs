using MangaUpdater.Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace MangaUpdater.API.Exceptions;

public class AuthExceptionHandler : IExceptionHandler
{
    public bool CanHandle(Exception exception) => exception is AuthorizationException;

    public ProblemDetails Handle(HttpContext httpContext, Exception exception)
    {
        return new ProblemDetails()
        {
            Title = "Unauthorized",
            Status = StatusCodes.Status401Unauthorized,
            Detail = exception.Message
        };
    }
}

public class NotFoundExceptionHandler : IExceptionHandler
{
    public bool CanHandle(Exception exception) => exception is NotFoundException;

    public ProblemDetails Handle(HttpContext httpContext, Exception exception)
    {
        return new ProblemDetails()
        {
            Title = "Not Found",
            Status = StatusCodes.Status404NotFound,
            Detail = exception.Message
        };
    }
}

public class ValidationExceptionHandler : IExceptionHandler
{
    public bool CanHandle(Exception exception) => exception is ValidationException;

    public ProblemDetails Handle(HttpContext httpContext, Exception exception)
    {
        return new ProblemDetails()
        {
            Title = "Validations errors occured",
            Status = StatusCodes.Status400BadRequest,
            Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1"
        };
    }
}

public class BadRequestExceptionHandler : IExceptionHandler
{
    public bool CanHandle(Exception exception) => exception is BadRequestException;

    public ProblemDetails Handle(HttpContext httpContext, Exception exception)
    {
        return new ProblemDetails()
        {
            Title = "Invalid Request",
            Status = StatusCodes.Status400BadRequest,
            Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1"
        };
    }
}