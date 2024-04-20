using System.ComponentModel.DataAnnotations;

namespace MangaUpdater.Core.Models;

public class UserAuthenticateModel
{
    [Required(ErrorMessage = "Required field")]
    [EmailAddress(ErrorMessage = "Invalid field")]
    public required string Email { get; set; }

    [Required(ErrorMessage = "Required field")]
    [StringLength(32, ErrorMessage = "The {0} field must have between 8 and 32 characters", MinimumLength = 8)]
    public required string Password { get; set; }
}