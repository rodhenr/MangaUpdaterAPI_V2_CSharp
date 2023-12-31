﻿namespace MangaUpdater.Application.Models.Register;

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

    public override string ToString()
    {
        return string.Join(", ", ErrorList);
    }
}