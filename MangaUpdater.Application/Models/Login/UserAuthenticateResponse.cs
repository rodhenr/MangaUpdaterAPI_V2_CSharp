using System.Text.Json.Serialization;

namespace MangaUpdater.Application.Models.Login;

public class UserAuthenticateResponse
{
    public UserAuthenticateResponse()
    {
        ErrorList = new List<string>();
    }

    public UserAuthenticateResponse(string? userName, string? userAvatar, string? accessToken, string? refreshToken,
        bool isAdmin) : this()
    {
        AccessToken = accessToken;
        RefreshToken = refreshToken;
        UserName = userName;
        UserAvatar = userAvatar;
        IsAdmin = isAdmin;
    }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? UserName { get; private set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? UserAvatar { get; private set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? AccessToken { get; private set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? RefreshToken { get; private set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? IsAdmin { get; private set; }

    [JsonIgnore] public bool IsSuccess => ErrorList.Count == 0;

    [JsonPropertyName("IsSuccess")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? SerializationIsSuccess => IsSuccess ? null : false;

    [JsonIgnore] public List<string> ErrorList { get; }

    [JsonPropertyName("ErrorList")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public List<string>? SerializationErrorList => ErrorList.Count > 0 ? ErrorList : null;

    public void AddError(string error) => ErrorList.Add(error);

    public override string ToString()
    {
        return string.Join(", ", ErrorList);
    }
};