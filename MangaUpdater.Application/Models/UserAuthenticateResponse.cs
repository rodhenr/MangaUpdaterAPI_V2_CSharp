namespace MangaUpdater.Application.Models;

public record UserAuthenticateResponse(DateTime? ExpirationDate, string? Token, bool IsSuccess);