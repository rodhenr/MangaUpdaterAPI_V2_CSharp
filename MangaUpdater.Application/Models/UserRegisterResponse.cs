namespace MangaUpdater.Application.Models;

public class UserRegisterResponse
{
    private UserRegisterResponse()
    {
        ErrorList = new List<string>();
    }

    public UserRegisterResponse(bool success = true) : this()
    {
        Success = success;
    }

    public bool Success { get; set; }
    public List<string> ErrorList { get; }

    public void AddErrors(IEnumerable<string> errors) => ErrorList.AddRange(errors);
}