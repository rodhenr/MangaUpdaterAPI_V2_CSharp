namespace MangaUpdaterAPI.Models;

public class User
{
    public User(int id, string name, string email, string avatar)
    {
        Id = id;
        Name = name;
        Email = email;
        Avatar = avatar;
    }

    public int Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string Avatar { get; set; }
}
