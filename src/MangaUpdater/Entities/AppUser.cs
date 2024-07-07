using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace MangaUpdater.Entities;

public class AppUser : IdentityUser
{
    [Column(TypeName = "nvarchar(MAX)")]
    [StringLength(int.MaxValue)]
    [Unicode(false)]
    public required string Avatar { get; set; }
}