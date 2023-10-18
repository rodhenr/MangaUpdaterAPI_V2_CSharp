using Microsoft.AspNetCore.Identity;

namespace MangaUpdater.Infra.Data.Identity;

public class AppUser : IdentityUser
{
    public required string Avatar { get; set; }
}