using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MangaUpdater.Core.Common.Exceptions;

public interface IExceptionHandler
{
    bool CanHandle(Exception exception);
    ProblemDetails Handle(HttpContext httpContext, Exception exception);
}