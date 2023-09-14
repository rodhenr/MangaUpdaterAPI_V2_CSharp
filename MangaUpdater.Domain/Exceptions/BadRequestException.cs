namespace MangaUpdater.Domain.Exceptions;

public class BadRequestException: Exception
{
    public BadRequestException(): base("Resource not found")
    {
    }
    
    public BadRequestException(string message): base(message)
    {
    }
}