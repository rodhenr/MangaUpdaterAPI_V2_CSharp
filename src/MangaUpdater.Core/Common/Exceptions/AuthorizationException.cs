namespace MangaUpdater.Core.Common.Exceptions;

public class AuthorizationException : Exception
{
    public AuthorizationException() : base("Invalid username or password")
    {
    }

    public AuthorizationException(string message) : base(message)
    {
    }
}