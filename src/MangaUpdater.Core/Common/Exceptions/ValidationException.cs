namespace MangaUpdater.Core.Common.Exceptions;

public class ValidationException : Exception
{
    public ValidationException() : base("Validation errors occured")
    {
    }

    public ValidationException(string message) : base(message)
    {
    }
}