using System.ComponentModel.DataAnnotations;

namespace MangaUpdater.Core.Models;

public record ChangeEmailModel
{
    [Required(ErrorMessage = "Required field")]
    [EmailAddress(ErrorMessage = "Invalid field")]
    public required string Email { get; init; }
    
    [Required(ErrorMessage = "Required field")]
    [StringLength(32, ErrorMessage = "The {0} field must have between 8 and 32 characters", MinimumLength = 8)]
    public required string Password  { get; init; }
    
    [Required(ErrorMessage = "Required field")]
    [Compare(nameof(Password), ErrorMessage = "Password must be the same")]
    public required string ConfirmationPassword { get; init; }
}