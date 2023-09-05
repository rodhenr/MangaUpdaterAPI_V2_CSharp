using Microsoft.AspNetCore.Mvc;

namespace MangaUpdater.API.Exceptions;

public interface IExceptionHandler
{
    bool CanHandle(Exception exception);
    ProblemDetails Handle(HttpContext httpContext, Exception exception);
}