using System.Text.Json.Serialization;

namespace MangaUpdater.Application.Models.Login;

public class UserAuthenticateResponse
{
    public UserAuthenticateResponse()
    {
        ErrorList = new List<string>();
    }

    public UserAuthenticateResponse(DateTime? expirationDate, string? token) : this()
    {
        ExpirationDate = expirationDate;
        Token = token;
    }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public DateTime? ExpirationDate { get; private set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Token { get; private set; }

    public bool IsSuccess => ErrorList.Count == 0;

    public List<string> ErrorList { get; }

    public void AddError(string error) => ErrorList.Add(error);

    public override string ToString()
    {
        return string.Join(", ", ErrorList);
    }
};