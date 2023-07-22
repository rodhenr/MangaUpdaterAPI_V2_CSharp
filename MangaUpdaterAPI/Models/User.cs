using System.ComponentModel.DataAnnotations;

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

    [MaxLength(50)]
    public string Name { get; set; }

    [MaxLength(50)]
    public string Email { get; set; }

    [MaxLength(200)]
    public string Avatar { get; set; }
}
