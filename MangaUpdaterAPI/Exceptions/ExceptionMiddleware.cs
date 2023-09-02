namespace MangaUpdater.API.Exceptions;

public class ExceptionMiddleware : IMiddleware
{
    public ExceptionMiddleware()
    {
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            // TODO:Implement types of errors
            context.Response.StatusCode = 400;
            await context.Response.WriteAsJsonAsync(ex.Message);
        }
    }
}