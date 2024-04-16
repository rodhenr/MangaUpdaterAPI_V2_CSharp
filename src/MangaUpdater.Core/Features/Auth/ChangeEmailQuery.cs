namespace MangaUpdater.Core.Auth;

public record ChangeEmailQuery(string newEmail, string password, string confirmationPassword);