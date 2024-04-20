namespace MangaUpdater.Core.Models;

public class UserRegisterResponseModel
{
    private UserRegisterResponseModel()
    {
        ErrorList = new List<string>();
    }

    public UserRegisterResponseModel(bool success = true) : this()
    {
        Success = success;
    }

    public bool Success { get; set; }
    private List<string> ErrorList { get; }

    public void AddErrors(IEnumerable<string> errors) => ErrorList.AddRange(errors);

    public override string ToString()
    {
        return string.Join(", ", ErrorList);
    }
}