namespace MangaUpdater.Application.Models;

public record ChangeEmailQuery(string newEmail, string password, string confirmationPassword);