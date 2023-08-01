using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace MangaUpdater.Domain.Entities;

public class User
{
    public User(string name, string email, string avatar)
    {
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

    [JsonIgnore]
    public ICollection<UserManga> UserMangas { get; set; }
}
