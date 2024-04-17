using System.ComponentModel.DataAnnotations;

namespace MangaUpdater.Core.Models;

public class UserRegisterModel
{
    [Required(ErrorMessage = "Required field")]
    [StringLength(30, ErrorMessage = "The {0} field must have between 4 and 30 characters", MinimumLength = 4)]
    public string UserName { get; set; }

    [Required(ErrorMessage = "Required field")]
    [EmailAddress(ErrorMessage = "Invalid field")]
    public string Email { get; set; }

    [Required(ErrorMessage = "Required field")]
    [StringLength(32, ErrorMessage = "The {0} field must have between 8 and 32 characters", MinimumLength = 8)]
    public string Password { get; set; }

    [Compare(nameof(Password), ErrorMessage = "Password must be the same")]
    public string ConfirmationPassword { get; set; }
}