namespace MangaUpdater.Infra.Data.Identity;

public record UserAuthenticateResponse
{
    public UserAuthenticateResponse(DateTime? expirationDate, string? token, bool isSuccess)
    {
        ExpirationDate = expirationDate;
        Token = token;
        IsSuccess = isSuccess;
    }

    public DateTime? ExpirationDate { get; set; }
    public string? Token { get; set; }
    public bool IsSuccess { get; set; }
}
