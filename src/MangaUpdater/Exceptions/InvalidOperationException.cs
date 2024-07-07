namespace MangaUpdater.Exceptions;

public class InvalidOperationException: Exception
{
    public InvalidOperationException(): base("Invalid Operation")
    {
    }
    
    public InvalidOperationException(string message): base(message)
    {
    }
}