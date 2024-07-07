namespace MangaUpdater.Exceptions;

public class EntityNotFoundException: Exception
{
    public EntityNotFoundException(): base("Entity not found")
    {
    }
    
    public EntityNotFoundException(string message): base(message)
    {
    }
}