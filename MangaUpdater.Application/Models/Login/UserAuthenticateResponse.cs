using System.Text.Json.Serialization;

namespace MangaUpdater.Application.Models.Login;

public class UserAuthenticateResponse
{
    public UserAuthenticateResponse()
    {
        ErrorList = new List<string>();
    }

    public UserAuthenticateResponse(string? accessToken, string? refreshToken) : this()
    {
        AccessToken = accessToken;
        RefreshToken = refreshToken;
    }
    
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? AccessToken { get; private set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? RefreshToken { get; private set; }

    public bool IsSuccess => ErrorList.Count == 0;
    public List<string> ErrorList { get; } //TODO: JsonIgnore when its empty

    public void AddError(string error) => ErrorList.Add(error);

    public override string ToString()
    {
        return string.Join(", ", ErrorList);
    }
};