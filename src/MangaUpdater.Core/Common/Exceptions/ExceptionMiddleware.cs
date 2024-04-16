using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MangaUpdater.Core.Common.Exceptions;

public class ExceptionMiddleware : IMiddleware
{
    private readonly IEnumerable<IExceptionHandler> _handlers;

    public ExceptionMiddleware(IEnumerable<IExceptionHandler> handlers)
    {
        _handlers = handlers;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            var handler = _handlers.FirstOrDefault(h => h.CanHandle(ex));
            var problemsDetails = handler?.Handle(context, ex) ?? new ProblemDetails
            {
                Title = "Error",
                Status = StatusCodes.Status500InternalServerError,
                Detail = ex.Message
            };

            context.Response.StatusCode = problemsDetails.Status!.Value;
            await context.Response.WriteAsJsonAsync(problemsDetails);
        }
    }
}